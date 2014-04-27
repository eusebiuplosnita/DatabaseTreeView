using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DatabaseTreeView
{
    public partial class Form1 : Form
    {
        public Tree tree;

        public Form1()
        {
            tree = new Tree();
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            dataGridView1.Columns.Add("Name", "Name");
            dataGridView1.Columns.Add("Size", "Size");
            dataGridView1.Columns.Add("Type", "Type");
            dataGridView1.Columns.Add("DateModified", "DateModified");
            dataGridView1.Columns.Add("ParentID", "ParentID");

            PopuleazaTreeView();
        }

        public void PopuleazaTreeView()
        {
            treeView1.Nodes.Clear();
            Node root = tree.GetRoot();
            Queue<Node> coada = new Queue<Node>();
            Queue<TreeNode> coada_treenode = new Queue<TreeNode>();
            coada.Enqueue(root);
            TreeNode root_tree = new TreeNode(root.Name);
            coada_treenode.Enqueue(root_tree);
            Node nod;
            TreeNode treenode;
            while (coada.Count != 0)
            {
                nod = coada.Dequeue();
                treenode = coada_treenode.Dequeue();
                foreach (Node n in nod.descendenti_directi)
                {
                    if (n.Type.StartsWith("folder"))
                    {
                        coada.Enqueue(n);
                        TreeNode tn = new TreeNode(n.Name);
                        treenode.Nodes.Add(tn);
                        coada_treenode.Enqueue(tn);
                    }
                }
            }

            treeView1.Nodes.Add(root_tree);
            treeView1.NodeMouseClick += new TreeNodeMouseClickEventHandler(treeView1_NodeMouseClick);
        }

        void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            dataGridView1.Rows.Clear();
            Node nod = tree.GetNode(e.Node.Text);
            if (nod != null)
            {
                foreach (Node n in nod.descendenti_directi)
                {
                    string[] row = new string[5];
                    row[0] = n.Name;
                    row[1] = n.Size;
                    row[2] = n.Type;
                    row[3] = n.dateModified;
                    row[4] = n.parentId.ToString();
                    dataGridView1.Rows.Add(row);
                }
            }

            //dataGridView1.RowHeaderMouseDoubleClick += new DataGridViewCellMouseEventHandler(dataGridView1_RowHeaderMouseDoubleClick);
            dataGridView1.RowHeaderMouseClick += new DataGridViewCellMouseEventHandler(dataGridView1_RowHeaderMouseClick);
        }

        public Node node_selected; 

        void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            node_selected = new Node();
            node_selected.Name = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            node_selected.Size = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            node_selected.Type = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            node_selected.dateModified = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            node_selected.parentId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[4].Value.ToString());
        }

        

        private void buttonInsert_Click(object sender, EventArgs e)
        {
            SqlConnection sqlCon = CreateConnection();
            SqlCommand command = sqlCon.CreateCommand();
            SqlTransaction transaction;

            // Start a local transaction.
            transaction = sqlCon.BeginTransaction("InsertTransaction");

            // Must assign both transaction object and connection 
            // to Command object for a pending local transaction
            command.Connection = sqlCon;
            command.Transaction = transaction;

            try
            {
                command.CommandText =
                    "Insert into Table1 (Name, Type, Size, DateModified, ParentId) VALUES (@pName, @pSize, @pType, @pDateModified, @pParentID)";
                command.Parameters.AddWithValue("@pName", node_selected.Name.ToString());
                command.Parameters.AddWithValue("@pSize", node_selected.Size.ToString());
                command.Parameters.AddWithValue("@pType", node_selected.Type.ToString());
                command.Parameters.AddWithValue("@pDateModified", node_selected.dateModified.ToString());
                command.Parameters.AddWithValue("@pParentID", node_selected.parentId.ToString());
                command.ExecuteNonQuery();

                // Attempt to commit the transaction.
                transaction.Commit();
            }
            catch (Exception ex)
            {
                try
                {
                    transaction.Rollback();
                }
                catch (Exception ex2)
                {
                }
            }
            sqlCon.Close();

            tree = new Tree();
            PopuleazaTreeView();
        }

        private SqlConnection CreateConnection()
        {
            string connectionString = @"Data Source=.\SQLEXPRESS;AttachDbFilename=C:\Documents and Settings\Daniel.COJO\My Documents\Visual Studio 2010\Projects\DatabaseTreeView\DatabaseTreeView\HardDisk.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();

            return sqlCon;
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            SqlConnection sqlCon = CreateConnection();
            SqlCommand command = sqlCon.CreateCommand();
            SqlTransaction transaction;

            // Start a local transaction.
            transaction = sqlCon.BeginTransaction("UpdateTransaction");

            // Must assign both transaction object and connection 
            // to Command object for a pending local transaction
            command.Connection = sqlCon;
            command.Transaction = transaction;

            try
            {
                
                command.CommandText =
                    "UPDATE Table1 SET Size=@pSize, Type=@pType, DateModified=@pDateModified, ParentId=@pParentID WHERE Name=@pName";
                command.Parameters.AddWithValue("@pName",node_selected.Name.ToString());
                command.Parameters.AddWithValue("@pSize", node_selected.Size.ToString());
                command.Parameters.AddWithValue("@pType", node_selected.Type.ToString());
                command.Parameters.AddWithValue("@pDateModified", node_selected.dateModified.ToString());
                command.Parameters.AddWithValue("@pParentID", node_selected.parentId.ToString());
                command.ExecuteNonQuery();

                // Attempt to commit the transaction.
                transaction.Commit();
            }
            catch (Exception ex)
            {
                try
                {
                    transaction.Rollback();
                }
                catch (Exception ex2)
                {
                }
            }
            sqlCon.Close();

            tree = new Tree();
            PopuleazaTreeView();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            SqlConnection sqlCon = CreateConnection();
            SqlCommand command = sqlCon.CreateCommand();
            SqlTransaction transaction;

            // Start a local transaction.
            transaction = sqlCon.BeginTransaction("DeleteTransaction");

            // Must assign both transaction object and connection 
            // to Command object for a pending local transaction
            command.Connection = sqlCon;
            command.Transaction = transaction;

            try
            {

                command.CommandText =
                    "DELETE Table1 WHERE Name=@pName AND Size=@pSize AND Type=@pType AND DateModified=@pDateModified AND ParentId=@pParentID";
                command.Parameters.AddWithValue("@pName", node_selected.Name.ToString());
                command.Parameters.AddWithValue("@pSize", node_selected.Size.ToString());
                command.Parameters.AddWithValue("@pType", node_selected.Type.ToString());
                command.Parameters.AddWithValue("@pDateModified", node_selected.dateModified.ToString());
                command.Parameters.AddWithValue("@pParentID", node_selected.parentId.ToString());
                command.ExecuteNonQuery();

                // Attempt to commit the transaction.
                transaction.Commit();
            }
            catch (Exception ex)
            {
                try
                {
                    transaction.Rollback();
                }
                catch (Exception ex2)
                {
                }
            }
            sqlCon.Close();

            tree = new Tree();
            PopuleazaTreeView();
        }

    }
}

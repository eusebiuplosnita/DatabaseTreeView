using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.SqlClient;

namespace DatabaseTreeView
{
    public class Tree
    {
        public ArrayList noduri;

        public Tree()
        {
            BuildTree();

        }

        public void BuildTree()
        {
            noduri = new ArrayList();
            SqlConnection sqlCon = CreateConnection();
            string command = "select Id, Name, Size, Type, DateModified, ParentId from Table1";
            SqlDataReader reader = ExecuteCommand(sqlCon, command);

            int id, idParent;
            string name, type, size, datemodified;

            while (reader.Read())
            {
                id = Convert.ToInt32(reader["Id"].ToString());
                name = reader["Name"].ToString();
                type = reader["Type"].ToString();
                size = reader["Size"].ToString();
                datemodified = reader["DateModified"].ToString();
                idParent = Convert.ToInt32(reader["ParentId"].ToString());

                Node node = new Node(id, name, type, size, datemodified, idParent);
                noduri.Add(node);
            }

            sqlCon.Close();

            Node root = GetRoot();

            Queue<Node> coada = new Queue<Node>();
            coada.Enqueue(root);
            Node nod;
            while (coada.Count != 0)
            {
                nod = coada.Dequeue();
                foreach (Node n in noduri)
                {
                    if (n.parentId == nod.id)
                    {
                        nod.descendenti_directi.Add(n);
                        coada.Enqueue(n);
                    }
                }
            }
        }

        public Node GetRoot()
        {
            Node root = null;
            foreach (Node n in noduri)
            {
                if (n.parentId == 0)
                {
                    root = n;
                }
            }
            return root;
        }

        public Node GetNode(string name)
        {
            foreach (Node n in noduri)
            {
                if (n.Name == name)
                    return n;
            }
            return null;
        }
        

        private SqlConnection CreateConnection()
        {
            string connectionString = @"Data Source=.\SQLEXPRESS;AttachDbFilename=C:\Documents and Settings\Daniel.COJO\My Documents\Visual Studio 2010\Projects\DatabaseTreeView\DatabaseTreeView\HardDisk.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();

            return sqlCon;
        }

        private SqlDataReader ExecuteCommand(SqlConnection sqlCon, string command)
        {
            SqlCommand sqlCmd = new SqlCommand(command, sqlCon);
            SqlDataReader read = sqlCmd.ExecuteReader();
            return read;
        }

    }
}

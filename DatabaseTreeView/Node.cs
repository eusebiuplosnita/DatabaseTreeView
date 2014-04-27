using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace DatabaseTreeView
{
    public class Node
    {
        public int id;
        public string Name;
        public string Type;
        public string Size;
        public string dateModified;
        public int parentId;
        public ArrayList descendenti_directi;

        public Node(int id, string name, string type, string size, string datemodified, int parentID)
        {
            this.id = id;
            this.Name = name;
            this.Type = type;
            this.Size = size;
            this.dateModified = datemodified;
            this.parentId = parentID;

            this.descendenti_directi = new ArrayList();

        }

        public Node()
        {
 
        }

    }
}

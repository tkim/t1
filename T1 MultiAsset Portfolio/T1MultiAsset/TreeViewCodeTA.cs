using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text.RegularExpressions;



namespace TreeViewControl
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TreeView		treeView1;
		private System.Windows.Forms.Button			buttonAddChild;
		private System.Windows.Forms.Button			buttonDeleteParent;
		private System.Windows.Forms.Button			btnAddSibling;
		private System.Windows.Forms.ContextMenu	contextMenuParent;
		private System.Windows.Forms.ContextMenu	contextMenuChild;
		private System.Windows.Forms.MenuItem		menuItem1;
		private System.Windows.Forms.MenuItem		menuItem2;
		private System.Windows.Forms.MenuItem		menuItem3;
		private System.Windows.Forms.MenuItem		menuItem4;
		private System.Windows.Forms.TextBox		textBox1; 
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			TreeNode tn			= new TreeNode("MS.NET");
			tn.ForeColor		= Color.Blue;
			treeView1.Nodes.Add(tn);
			treeView1.LabelEdit	= true;
			MakeButtonsEnableDisable(true,false,false,false);
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.buttonAddChild = new System.Windows.Forms.Button();
			this.buttonDeleteParent = new System.Windows.Forms.Button();
			this.contextMenuParent = new System.Windows.Forms.ContextMenu();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.btnAddSibling = new System.Windows.Forms.Button();
			this.contextMenuChild = new System.Windows.Forms.ContextMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// treeView1
			// 
			this.treeView1.ImageIndex = -1;
			this.treeView1.Location = new System.Drawing.Point(24, 8);
			this.treeView1.Name = "treeView1";
			this.treeView1.SelectedImageIndex = -1;
			this.treeView1.Size = new System.Drawing.Size(352, 216);
			this.treeView1.TabIndex = 0;
			this.treeView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseDown);
			this.treeView1.Click += new System.EventHandler(this.treeView1_Click);
			this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
			// 
			// buttonAddChild
			// 
			this.buttonAddChild.Location = new System.Drawing.Point(72, 264);
			this.buttonAddChild.Name = "buttonAddChild";
			this.buttonAddChild.TabIndex = 1;
			this.buttonAddChild.Text = "Add Child";
			this.buttonAddChild.Click += new System.EventHandler(this.buttonAddChild_Click);
			// 
			// buttonDeleteParent
			// 
			this.buttonDeleteParent.Location = new System.Drawing.Point(248, 264);
			this.buttonDeleteParent.Name = "buttonDeleteParent";
			this.buttonDeleteParent.Size = new System.Drawing.Size(72, 23);
			this.buttonDeleteParent.TabIndex = 1;
			this.buttonDeleteParent.Text = "Delete";
			this.buttonDeleteParent.Click += new System.EventHandler(this.buttonDeleteParent_Click);
			// 
			// contextMenuParent
			// 
			this.contextMenuParent.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							  this.menuItem2,
																							  this.menuItem3,
																							  this.menuItem4});
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 0;
			this.menuItem2.Text = "Add Child";
			this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 1;
			this.menuItem3.Text = "Add Sibling";
			this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 2;
			this.menuItem4.Text = "Delete";
			this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
			// 
			// btnAddSibling
			// 
			this.btnAddSibling.Location = new System.Drawing.Point(160, 264);
			this.btnAddSibling.Name = "btnAddSibling";
			this.btnAddSibling.TabIndex = 1;
			this.btnAddSibling.Text = "Add Sibling";
			this.btnAddSibling.Click += new System.EventHandler(this.btnAddSibling_Click);
			// 
			// contextMenuChild
			// 
			this.contextMenuChild.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							 this.menuItem1});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.Text = "Add Child";
			this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click_1);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(136, 232);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(120, 20);
			this.textBox1.TabIndex = 2;
			this.textBox1.Text = "";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(400, 293);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.textBox1,
																		  this.buttonAddChild,
																		  this.treeView1,
																		  this.buttonDeleteParent,
																		  this.btnAddSibling});
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		
		
		/// <summary>
		/// MEthod defn when the user clicks the Add Child button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonAddChild_Click(object sender, System.EventArgs e)
		{
			if(textBox1.Text != "")
				AddChildToTheNode();
			else
			{
				MessageBox.Show("Enter the Node Text to be added");
				textBox1.Focus();
			}
		}
		
		/// <summary>
		/// Mehtod defn when delete parent is clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonDeleteParent_Click(object sender, System.EventArgs e)
		{
			DeleteNode();
		}
		
		/// <summary>
		/// Method defn when the user clicks on the treeview control to open the context menu
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void treeView1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Right)
			{
				if( treeView1.SelectedNode.ForeColor == Color.Blue)
					contextMenuChild.Show(this,new Point(e.X,e.Y));
				else
					contextMenuParent.Show(this,new Point(e.X,e.Y));
			}
		}
		
		/// <summary>
		/// Method defn to add the sibling to the tree view control
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnAddSibling_Click(object sender, System.EventArgs e)
		{
			if(textBox1.Text != "")
				AddSiblingToTheNode();
			else
			{
				MessageBox.Show("Enter the Node Text to be added");
				textBox1.Focus();
			}
		}
		
		/// <summary>
		/// MEthod defn to enable disable the buttons
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void treeView1_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			textBox1.Text = "";
			if(e.Node.ForeColor == Color.Blue)
				MakeButtonsEnableDisable(true,false,false,false);
			else 
				MakeButtonsEnableDisable(true,true,true,false);
		}
		
        private void MakeButtonsEnableDisable(bool blnAddChild,bool blnDelete,bool blnAddSibling,bool blnAddParent)
		{
			buttonAddChild.Enabled		= blnAddChild;
			buttonDeleteParent.Enabled  = blnDelete;
			btnAddSibling.Enabled		= blnAddSibling;
		}

		private void menuItem2_Click(object sender, System.EventArgs e)
		{
			if(textBox1.Text != "")
				AddChildToTheNode();
			else
			{
				MessageBox.Show("Enter the Node Text to be added");
				textBox1.Focus();
			}
		}

		private void menuItem3_Click(object sender, System.EventArgs e)
		{
			if(textBox1.Text != "")
				AddSiblingToTheNode();
			else
			{
				MessageBox.Show("Enter the Node Text to be added");
				textBox1.Focus();
			}
		}

		private void menuItem4_Click(object sender, System.EventArgs e)
		{
			DeleteNode();
		}

		private void menuItem1_Click_1(object sender, System.EventArgs e)
		{
			if(textBox1.Text != "")
				AddChildToTheNode();
			else
			{
				MessageBox.Show("Enter the Node Text to be added");
				textBox1.Focus();
			}
		}

		private void AddChildToTheNode()
		{
			TreeNode tnode	= new TreeNode(textBox1.Text);
			treeView1.SelectedNode.Nodes.Add(tnode);
			treeView1.ExpandAll();
			if(treeView1.SelectedNode.Nodes.Count > 1 && treeView1.SelectedNode.ForeColor != Color.Blue)
				treeView1.SelectedNode.ForeColor = Color.Brown;
		}

		private void AddSiblingToTheNode()
		{
			TreeNode tnode	= new TreeNode(textBox1.Text);
			tnode.ForeColor = Color.Brown;
			treeView1.SelectedNode.Parent.Nodes.Add(tnode);
		}

		private void DeleteNode()
		{
			if(treeView1.SelectedNode.Nodes.Count == 0)
				treeView1.SelectedNode.Remove();
			else
				MessageBox.Show("First Remove all the child nodes");
		}

		private void treeView1_Click(object sender, System.EventArgs e)
		{
			textBox1.Text = "";
		}
	}
}

using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Collections;



namespace Org.Mentalis.Proxy {
	/// <summary>
	/// Summary description for ExtendedTreeNode.
	/// </summary>
	public class ExtendedTreeNode : TreeNode {

		public enum Protocol { HTTP, HTTPS };
		public Hashtable children;

		public ExtendedTreeNode(string name)	{
			//
			// TODO: Add constructor logic here
			//
			this.Text						= name;
			this.children					= new Hashtable();
		}
	}		// end class ExtendedTreeNode

	public class Utilities	{

		public enum Mode		{ one=1, two=2 };
		public enum Protocol	{ HTTP, HTTPS };

		public static Color getColourCode(Mode mode, Protocol p)	{
			Color theColour				= Color.Black;
			// Colour code the last node
			switch ( mode )	{
				case Mode.one :
					if ( p == Protocol.HTTP )
						 theColour		= Color.LightBlue;
					if ( p == Protocol.HTTPS )
						theColour		= Color.DarkBlue;
					break;

				case Mode.two :
					if ( p == Protocol.HTTP )
						theColour		= Color.LightSalmon;
					if ( p == Protocol.HTTPS )
						theColour		= Color.Red;
					break;
			}
			return theColour;
		}
		
		public static void AddURLToTreeView(string url, Mode mode, TreeView treeView, Hashtable htNodes)	{
			Regex r							= new Regex(@"^\s*(?<protocol>https?)://(?<path>.*)", RegexOptions.IgnoreCase);
			Match m							= r.Match(url);
			TreeNodeCollection nodes		= treeView.Nodes;
			if ( m.Success )	{
				Protocol p	= (Protocol)Enum.Parse(typeof(Protocol), m.Result("${protocol}"), true);
				char[] separators			= { '\\', '/' };
				ExtendedTreeNode etn		= null;
				foreach ( string path in m.Result("${path}").Split(separators) )	{
					if ( htNodes.ContainsKey(path) )	{
						etn					= (ExtendedTreeNode)htNodes[path];
					}
					else	{
						etn					= new ExtendedTreeNode(path);
						etn.ForeColor		= getColourCode(mode, p);
						nodes.Add(etn);
						htNodes.Add(path, etn);
					}
					// Expand the node in mode two
					if ( mode == Mode.two )
						etn.Expand();
					htNodes					= etn.children;
					nodes					= etn.Nodes;
				}		// next node
				etn.ForeColor				= getColourCode(mode, p);
				treeView.Refresh();
			}		// if valid url
		}		// end AddURLToTreeView()
		
	}		// end class Utilities
	
}		// end namespace SensePost.URL_Parser

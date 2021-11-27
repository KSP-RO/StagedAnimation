using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace StagedAnimation
{
	public static class PartEx
	{
		/// <summary>
		/// Returns the parent object of all the MODEL{} nodes.
		/// </summary>
		/// <param name="part"></param>
		/// <returns></returns>
		public static List<Transform> FindModelNodes( this Part part )
		{
			// Find the parent of all MODEL{} nodes.
			Transform modelParent = null;
			for( int i = 0; i < part.partTransform.childCount; i++ )
			{
				Transform child = part.partTransform.GetChild( i );
				if( child.gameObject.name == "model" )
				{
					modelParent = child;
					break;
				}
			}

			List<Transform> models = new List<Transform>();
			// Add the children (MODEL{} objects).
			for( int i = 0; i < modelParent.childCount; i++ )
            {
				models.Add( modelParent.GetChild( i ) );
            }

			return models;
		}
	}
}
#if UNITY_EDITOR
namespace InControl
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Text.RegularExpressions;
	using UnityEditor;
	using UnityEngine;


	[InitializeOnLoad]
	class NativeProfileListGenerator
	{
		static NativeProfileListGenerator()
		{
			if (!EditorApplication.isPlayingOrWillChangePlaymode)
			{
				DiscoverProfiles();
			}
		}


		static void DiscoverProfiles()
		{
			var nativeInputDeviceProfileType = typeof(NativeInputDeviceProfile);

			var names = new List<string>();

			foreach (var type in Reflector.AllAssemblyTypes)
			{
				if (type.IsSubclassOf( nativeInputDeviceProfileType ))
				{
					names.Add( type.FullName );
				}
			}

			names.Sort();

			var code2 = "";
			foreach (var name in names)
			{
				code2 += "\t\t\t\"" + name + "\"," + Environment.NewLine;
			}

			var instance = ScriptableObject.CreateInstance<NativeInputDeviceProfileList>();
			var filePath = AssetDatabase.GetAssetPath( MonoScript.FromScriptableObject( instance ) );
			UnityEngine.Object.DestroyImmediate( instance );

			const string code1 = @"namespace InControl
{
	using UnityEngine;


	public class NativeInputDeviceProfileList : ScriptableObject
	{
		public static readonly string[] Profiles = new string[]
		{
";

			const string code3 = @"		};
	}
}";

			var code = FixNewLines( code1 + code2 + code3 );
			if (PutFileContents( filePath, code ))
			{
				Debug.Log( "InControl has updated the native profiles list." );
			}
		}


		static string GetFileContents( string fileName )
		{
			var streamReader = new StreamReader( fileName );
			var fileContents = streamReader.ReadToEnd();
			streamReader.Close();

			return fileContents;
		}


		static bool PutFileContents( string filePath, string content )
		{
			var oldContent = GetFileContents( filePath );
			if (CompareIgnoringWhitespace( content, oldContent ))
			{
				return false;
			}

			var streamWriter = new StreamWriter( filePath );
			streamWriter.Write( content );
			streamWriter.Flush();
			streamWriter.Close();

			return true;
		}


		static string FixNewLines( string text )
		{
			return Regex.Replace( text, @"\r\n|\n", Environment.NewLine );
		}


		static bool CompareIgnoringWhitespace( string s1, string s2 )
		{
			return Regex.Replace( s1, @"\s", "" ) == Regex.Replace( s2, @"\s", "" );
		}
	}
}
#endif

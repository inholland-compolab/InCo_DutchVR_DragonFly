
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;

public class MegaFlowFGA
{
	static int index = 0;

	static public string MakeFileName(string file, ref int format)
	{
		string ret = "";
		format = 0;
		for ( int i = file.Length - 1; i >= 0; i-- )
		{
			char c = file[i];
			if ( Char.IsNumber(c) )
				format++;
			else
			{
				ret = file.Substring(0, i + 1);
				break;
			}
		}

		return ret;
	}

	public static MegaFlowFrame LoadFrame(string filename, int frame, string namesplit, int decformat)
	{
		MegaFlowFrame flow = null;

		string dir= Path.GetDirectoryName(filename);
		string file = Path.GetFileNameWithoutExtension(filename);

		file = MakeFileName(file, ref decformat);
#if false
		char[]	splits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

		string[] names;

		if ( namesplit.Length > 0 )
		{
			names = file.Split(namesplit[0]);
			names[0] += namesplit[0];
		}
		else
			names = file.Split(splits);

		if ( names.Length > 0 )
		{
			string newfname = dir + "/" + names[0] + frame.ToString("D" + decformat) + ".fga";
			flow = LoadFrame(newfname);
		}
#else
		if ( file.Length > 0 )
		{
			string newfname = dir + "/" + file + frame.ToString("D" + decformat) + ".fga";
			flow = LoadFrame(newfname);
		}
#endif
		return flow;
	}

#if false
	public static MegaFlowFrame LoadFrame(string filename, int frame)
	{
		MegaFlowFrame flow = null;

		char[]	splits = { '.' };

		string fname = filename; // use unity get path

		string[] names = fname.Split(splits);

		if ( names.Length > 0 )
		{
			string newfname = names[0] + "." + frame + ".fga";
			flow = LoadFrame(newfname);
		}

		return flow;
	}
#endif

	public static MegaFlowFrame LoadFrame(string filename)
	{
		MegaFlowFrame flow = null;

		//Debug.Log("FGA file " + filename);
		if ( File.Exists(filename) )
		{
			flow = ScriptableObject.CreateInstance<MegaFlowFrame>();
			flow.Init();
			LoadFGA(flow, filename);
		}

		return flow;
	}

	static Vector3 ReadV3(string[] vals)
	{
		Vector3 v = Vector3.zero;

		v.x = float.Parse(vals[index++]);
		v.y = float.Parse(vals[index++]);
		v.z = float.Parse(vals[index++]);

		return v;
	}

	static Vector3 ReadV3Adj(string[] vals)
	{
		Vector3 v = ReadV3(vals);

		v.z = -v.z;

		return v;
	}

	static public void LoadFGA(MegaFlowFrame flow, string filename)
	{
		StreamReader reader = File.OpenText(filename);

		string file = reader.ReadToEnd();

		string[] vals = file.Split(',');

		index = 0;

		flow.gridDim2[0] = (int)float.Parse(vals[index++]);
		flow.gridDim2[1] = (int)float.Parse(vals[index++]);
		flow.gridDim2[2] = (int)float.Parse(vals[index++]);

		Vector3 bmin = ReadV3(vals);
		Vector3 bmax = ReadV3(vals);

		flow.size = bmax - bmin;
		flow.gsize = flow.size;

		// griddim should have a name change
		flow.spacing.x = flow.size.x / flow.gridDim2[0];
		flow.spacing.y = flow.size.y / flow.gridDim2[1];
		flow.spacing.z = flow.size.z / flow.gridDim2[2];
		flow.oos.x = 1.0f / flow.spacing.x;
		flow.oos.y = 1.0f / flow.spacing.y;
		flow.oos.z = 1.0f / flow.spacing.z;

		//Debug.Log("spacing " + flow.spacing);
		//Debug.Log("griddim " + flow.gridDim2[0] + " " + flow.gridDim2[1] + " " + flow.gridDim2[2]);

		flow.vel.Clear();

		Vector3[] vels = new Vector3[flow.gridDim2[0] * flow.gridDim2[1] * flow.gridDim2[2]];

		for ( int z = 0; z < flow.gridDim2[2]; z++ )
		{
			for ( int y = 0; y < flow.gridDim2[1]; y++ )
			{
				for ( int x = 0; x < flow.gridDim2[0]; x++ )
					vels[(x * flow.gridDim2[2] * flow.gridDim2[1]) + ((flow.gridDim2[2] - z - 1) * flow.gridDim2[1]) + y] = ReadV3Adj(vals);
			}
		}

		flow.framenumber = 0;
		flow.vel.AddRange(vels);
		reader.Close();

		reader = null;
		GC.Collect();
	}

	static void WriteV3(StreamWriter file, Vector3 v)
	{
		file.Write(v.x.ToString("0.#####") + ",");
		file.Write(v.y.ToString("0.#####") + ",");
		file.Write(v.z.ToString("0.#####") + ",");
	}

	static void WriteV3Adj(StreamWriter file, Vector3 v)
	{
		v.z = -v.z;
		file.Write(v.x.ToString("0.#####") + ",");
		file.Write(v.y.ToString("0.#####") + ",");
		file.Write(v.z.ToString("0.#####") + ",");
	}

	static public void SaveFGA(MegaFlowFrame flow, string filename)
	{
		StreamWriter file = new StreamWriter(filename);

		file.Write(flow.gridDim2[0].ToString("0.#####") + ",");
		file.Write(flow.gridDim2[1].ToString("0.#####") + ",");
		file.Write(flow.gridDim2[2].ToString("0.#####") + ",");

		Vector3 sz = flow.size * 0.5f;
		WriteV3(file, -sz);
		WriteV3(file, sz);

		for ( int z = 0; z < flow.gridDim2[2]; z++ )
		{
			for ( int y = 0; y < flow.gridDim2[1]; y++ )
			{
				for ( int x = 0; x < flow.gridDim2[0]; x++ )
					WriteV3Adj(file, flow.vel[(x * flow.gridDim2[2] * flow.gridDim2[1]) + ((flow.gridDim2[2] - z - 1) * flow.gridDim2[1]) + y]);
			}
		}

		file.Close();
	}

	// NetCDF example
/*
netCDF volume {
dimensions:
   nx = 3;
   ny = 3;
   nz = 3;

variables:
   float field_data(nx, ny, nz);

data:
   field_data =
        0, 0, 0
        0, 0, 0
        0, 5, 0

        0, 0, 5
        0, 0, 0
        0, 0, 0

        5, 0, 0
        0, 0, 0
        0, 0, 0;
}
*/

	static public void SaveNetCDF(MegaFlowFrame flow, string filename)
	{
		StreamWriter file = new StreamWriter(filename);

		file.Write("netCDF volume {\n");
		file.Write("dimensions:\n");

		file.Write("\tnx = " + flow.gridDim2[0].ToString("0.#####") + ";\n");
		file.Write("\tny = " + flow.gridDim2[1].ToString("0.#####") + ";\n");
		file.Write("\tnz = " + flow.gridDim2[2].ToString("0.#####") + ";\n");

		file.Write("\nvariables:\n\t float field_data(nx, ny, nz);\n");
		file.Write("\ndata:\n");

		file.Write("\tfield_data =\n");

		for ( int z = 0; z < flow.gridDim2[2]; z++ )
		{
			for ( int y = 0; y < flow.gridDim2[1]; y++ )
			{
				for ( int x = 0; x < flow.gridDim2[0]; x++ )
				{
					WriteV3Adj(file, flow.vel[(x * flow.gridDim2[2] * flow.gridDim2[1]) + ((flow.gridDim2[2] - z - 1) * flow.gridDim2[1]) + y]);
				}
				file.Write("\n");
			}
			file.Write("\n\n");
		}

		file.Write("}");
		file.Close();
	}

	public static MegaFlowFrame LoadFrameNetCDF(string filename)
	{
		MegaFlowFrame flow = null;

		if ( File.Exists(filename) )
		{
			flow = ScriptableObject.CreateInstance<MegaFlowFrame>();
			flow.Init();
			LoadNetCDF(flow, filename);
		}

		return flow;
	}

	static public void LoadNetCDF(MegaFlowFrame flow, string filename)
	{
		StreamReader reader = File.OpenText(filename);

		// Cant do this way will take to long, need to read from stream

		char[] split = { '=', ';' };
		reader.ReadLine();
		string dim = reader.ReadLine();
		if ( dim.Contains("dimensions") )
		{
			string line = reader.ReadLine();
			string[] parts = line.Split(split);
			flow.gridDim2[0] = int.Parse(parts[1]);
			line = reader.ReadLine();
			parts = line.Split(split);
			flow.gridDim2[1] = int.Parse(parts[1]);
			line = reader.ReadLine();
			parts = line.Split(split);
			flow.gridDim2[2] = int.Parse(parts[1]);

			flow.size = new Vector3(flow.gridDim2[0], flow.gridDim2[1], flow.gridDim2[2]);
			flow.gsize = flow.size;

			flow.spacing.x = flow.size.x / flow.gridDim2[0];
			flow.spacing.y = flow.size.y / flow.gridDim2[1];
			flow.spacing.z = flow.size.z / flow.gridDim2[2];
			flow.oos.x = 1.0f / flow.spacing.x;
			flow.oos.y = 1.0f / flow.spacing.y;
			flow.oos.z = 1.0f / flow.spacing.z;
		}

		reader.ReadLine();
		reader.ReadLine();
		reader.ReadLine();
		reader.ReadLine();
		reader.ReadLine();
		string data = reader.ReadLine();

		Vector3[] vels = new Vector3[flow.gridDim2[0] * flow.gridDim2[1] * flow.gridDim2[2]];

		bool cancel = false;

		if ( data.Contains("field_data") )
		{
			flow.vel.Clear();

			// Read data in
			Vector3 rval = Vector3.zero;
			string[] vals;

			for ( int z = 0; z < flow.gridDim2[2]; z++ )
			{
				if ( !EditorUtility.DisplayCancelableProgressBar("Importing NetCDF File", "Importing Slice " + z + " of " + flow.gridDim2[2], (float)z / (float)flow.gridDim2[2]) )
				{
					for ( int y = 0; y < flow.gridDim2[1]; y++ )
					{
						while ( true )
						{
							vals = reader.ReadLine().Split(',');
							if ( vals.Length > 1 )
								break;
						}
						int index = 0;
						for ( int x = 0; x < flow.gridDim2[0]; x++ )
						{
							rval.x = float.Parse(vals[index++]);
							rval.y = float.Parse(vals[index++]);
							rval.z = -float.Parse(vals[index++]);
							vels[(x * flow.gridDim2[2] * flow.gridDim2[1]) + ((flow.gridDim2[2] - z - 1) * flow.gridDim2[1]) + y] = rval;
						}
					}
				}
				else
				{
					cancel = true;
					break;
				}
			}
		}

		EditorUtility.ClearProgressBar();
		if ( !cancel )
		{
			flow.framenumber = 0;
			flow.vel.AddRange(vels);
		}

		reader.Close();

		reader = null;
		GC.Collect();

		return;
	}
}

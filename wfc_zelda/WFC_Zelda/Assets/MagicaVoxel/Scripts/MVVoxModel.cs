using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Diagnostics;
using System;

public class MVVoxModel : MonoBehaviour {
    
	// for animations, voxels can later be combined into individual layers
	[HideInInspector]
	public bool ed_importAsIndividualVoxels = false;

	// actually requred for instantiation
	public string ed_filePath = "";
  string filePath2 = @".\\Assets\dungeon_conf\output\zelda.vox";
 // GameObject textFile = Resources.Load("dungeon_conf/output/zelda");
  public GameObject par;
	[HideInInspector]
	public MVMainChunk vox;


	[Range(0.01f, 5.0f)]
	public float sizePerVox = 5.0f;

	public Material voxMaterial = null;

	public Transform meshOrigin = null;

    [Tooltip("If the vox file contains a palette, should it be converted to a texture?")]
    public bool paletteToTex = false;

	public void ClearVoxMeshes() {
		MVVoxModelMesh[] subMeshes = this.gameObject.GetComponentsInChildren<MVVoxModelMesh> ();
		foreach (MVVoxModelMesh subMesh in subMeshes)
			GameObject.DestroyImmediate (subMesh.gameObject);

		MVVoxModelVoxel[] subVoxels = this.gameObject.GetComponentsInChildren<MVVoxModelVoxel> ();
		foreach (MVVoxModelVoxel v in subVoxels)
			GameObject.DestroyImmediate (v.gameObject);

	}

	public void LoadVOXFile(string path, bool asIndividualVoxels) {
		ClearVoxMeshes ();

		if (path != null && path.Length > 0)
		{
			MVMainChunk v = MVImporter.LoadVOX (path);

			if (v != null) {
				Material mat = (this.voxMaterial != null) ? this.voxMaterial : MVImporter.DefaultMaterial;
                if (paletteToTex)
                    mat.mainTexture = v.PaletteToTexture();

				if (!asIndividualVoxels) {

					if (meshOrigin != null)
						MVImporter.CreateVoxelGameObjects(v, this.gameObject.transform, mat, sizePerVox, meshOrigin.localPosition);
					else
						MVImporter.CreateVoxelGameObjects (v, this.gameObject.transform, mat, sizePerVox);

				} else {

					if (meshOrigin != null)
						MVImporter.CreateIndividualVoxelGameObjects(v, this.gameObject.transform, mat, sizePerVox, meshOrigin.localPosition);
					else
						MVImporter.CreateIndividualVoxelGameObjects (v, this.gameObject.transform, mat, sizePerVox);

				}

				this.vox = v;
			}


		} else {
			print ("[MVVoxModel] Invalid file path");
		}
	}

	public void LoadVOXData(byte[] data, bool asIndividualVoxels) {
		ClearVoxMeshes ();

		MVMainChunk v = MVImporter.LoadVOXFromData(data);

		if (v != null) {
			Material mat = (this.voxMaterial != null) ? this.voxMaterial : MVImporter.DefaultMaterial;

            if (paletteToTex)
                mat.mainTexture = v.PaletteToTexture();

			if (!asIndividualVoxels) {

				MVImporter.CreateVoxelGameObjects (v, this.gameObject.transform, mat, sizePerVox);

			} else {

				MVImporter.CreateIndividualVoxelGameObjects (v, this.gameObject.transform, mat, sizePerVox);

			}

			this.vox = v;
		}

	}

    private int executeExternalProgram()
    {
        try
        {
            Process myProcess = new Process();
            myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            myProcess.StartInfo.CreateNoWindow = true;
            myProcess.StartInfo.UseShellExecute = false;
            myProcess.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
            myProcess.StartInfo.FileName = @".\\Assets\\tools\\DeBroglie.Console.exe";
            //myProcess.StartInfo.FileName = "C:\\Windows\\system32\\cmd.exe";
            //string path = @"..\\..\\tools\\DeBroglie.Console.exe" + @"..\\..\\dungeon_conf\\zelda.json";
            myProcess.StartInfo.Arguments = @".\\Assets\\dungeon_conf\\zelda.json";
            myProcess.EnableRaisingEvents = true;
            myProcess.StartInfo.RedirectStandardOutput = true;
            myProcess.Start();
            string output = myProcess.StandardOutput.ReadToEnd();
            myProcess.WaitForExit();
            int ExitCode = myProcess.ExitCode;
            print(output);
            print("the exit code : "+ExitCode);
            return ExitCode;
        }
        catch (Exception e)
        {
            print(e);
            return 1; //1 means error
        }
    }

    public bool reimportOnStart = true;
	void Start()
	{

        executeExternalProgram();

        if (reimportOnStart) {
			LoadVOXFile (filePath2, ed_importAsIndividualVoxels);
      

    }
  }
}

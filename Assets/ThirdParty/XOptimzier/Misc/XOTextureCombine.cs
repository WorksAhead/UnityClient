#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace XOpt
{
    [ExecuteInEditMode]
    public class MaterialMeshes
    {
        public Material Material;

        public List<MeshFilter> MeshFilters = new List<MeshFilter>();

        public List<Mesh> Meshes = new List<Mesh>();

        public List<Matrix4x4> Transform = new List<Matrix4x4>();
    }

    [ExecuteInEditMode]
    public class XOTextureCombine
    {

        static public Texture2D normatlas;
        static public Rect normrect;
        static public string atlaspath;
        static public List<Texture2D> specTextures = new List<Texture2D>();

        static public List<string> atlasType = new List<string>();
        static public int typeIndex = 0;
        static public int index = 0;
        static public int atlasWidth = 0;
        static public int atlasHeight = 0;
        static public Texture2D atlas;
        static public string[] coords;
        static public Rect[] rect;
        static public List<string> textureList = new List<string>();
        static public bool added = false;
        static public int oldTypeIndex = 3;
        static public bool test = false;
        static public bool adjusting = false;
        static public Material atlasMaterial = null;
        static public string matpath;


        static public Dictionary<string, string> Names2Coord = new Dictionary<string, string>();
        static public List<string> _textureCoords = new List<string>();





        public static void TryCombine(UnityEngine.GameObject[] gos)
        {
            if (gos != null)
            {
                specTextures.Clear();
                UnityEngine.GameObject shaderroot = null;
                for (int i = 0; i < gos.Length; i++)
                {
                    if (shaderroot == null)
                    {
                        shaderroot = gos[i].transform.parent.gameObject;
                    }
                    //Debug.Log(gos[i].name);
                    XOCp_MaterialRoot mr = gos[i].GetComponent<XOCp_MaterialRoot>();
                    if (mr != null)
                    {
                        if (mr.maintxt != null)
                        {
                            specTextures.Add(mr.maintxt);
                            string path = AssetDatabase.GetAssetPath(specTextures[i]);
                            TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
                            textureImporter.isReadable = true;
                            AssetDatabase.ImportAsset(path);
                            AssetDatabase.Refresh();
                        }
                    }
                }

                matpath = string.Empty;
                Names2Coord.Clear();
                SetupAtlas();
                AtlasCoordinatesDocument();
                SaveAtlas(shaderroot.name);
            }
        }







        public static void Combine(UnityEngine.GameObject[] gos)
        {
            if (gos != null)
            {
                specTextures.Clear();
                UnityEngine.GameObject shaderroot = null;
                for (int i = 0; i < gos.Length; i++)
                {
                    if (shaderroot == null)
                    {
                        shaderroot = gos[i].transform.parent.gameObject;
                    }
                    //Debug.Log(gos[i].name);
                    XOCp_MaterialRoot mr = gos[i].GetComponent<XOCp_MaterialRoot>();
                    if (mr != null)
                    {
                        if (mr.maintxt != null)
                        {
                            specTextures.Add(mr.maintxt);
                            string path = AssetDatabase.GetAssetPath(specTextures[i]);
                            TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
                            textureImporter.isReadable = true;
                            AssetDatabase.ImportAsset(path);
                            AssetDatabase.Refresh();
                        }
                    }
                }

                matpath = string.Empty;

                SetupAtlas();

                AtlasCoordinatesDocument();

                SaveAtlas(shaderroot.name);
                ReadSetup();

                UpdateUVs(gos);
                UpdateNodeName(gos);
                Debug.Log("Success Combine");


            }
        }

        public static void UpdateNodeName(UnityEngine.GameObject[] gos)
        {

        }


        public static void UpdateUVs(UnityEngine.GameObject[] gos)
        {
            if (gos != null)
            {
                for (int i = 0; i < gos.Length; i++)
                {
                    //Debug.Log("gos.name="+gos[i].name);

                    int cc = gos[i].transform.childCount;
                    if (cc > 0)
                    {
                        for (int j = 0; j < cc; j++)
                        {
                            UnityEngine.GameObject go = gos[i].transform.GetChild(j).gameObject;
                            if (go != null)
                            {
                                MeshRenderer lMeshRenderer = go.GetComponent<MeshRenderer>();
                                if (lMeshRenderer != null)
                                {
                                    //改uv
                                    RealUpdateUV(go, lMeshRenderer.sharedMaterial.mainTexture.name);

                                    //改材质
                                    //Debug.Log("matpath="+matpath);
                                    lMeshRenderer.sharedMaterial = AssetDatabase.LoadAssetAtPath(matpath, typeof(Material)) as Material;


                                }
                            }
                        }
                    }
                }
            }
        }

        public static void SetupAtlas()
        {
            if (specTextures.Count > 64)
            {
                EditorUtility.DisplayDialog("Exceeded safe texture limit", "If you experience quality loss remake atlas with fewer textures.", "Cancel");
            }
            atlas = new Texture2D(4096, 4096);
            rect = atlas.PackTextures(specTextures.ToArray(), 0, 8192);
            atlas.name = "New Atlas";
            atlas.Apply();
        }

        public static void SaveAtlas(string shadername)
        {
            // Opens a file selection dialog for a PNG file and saves a selected texture to the file.
            atlaspath = EditorUtility.SaveFilePanelInProject("Save texture as PNG",
                                                            atlas.name + ".png",
                                                            "png",
                                                            "Please enter a file name to save the texture to");
            if (atlaspath.Length != 0)
            {
                // Convert the texture to a format compatible with EncodeToPNG
                if (atlas.format != TextureFormat.ARGB32 && atlas.format != TextureFormat.RGB24)
                {
                    Texture2D newTexture = new Texture2D(atlas.width, atlas.height);
                    newTexture.SetPixels32(atlas.GetPixels32(0), 0);
                    atlas = newTexture;
                }
                //first write specs
                byte[] bytes = atlas.EncodeToPNG();
                FileStream file = File.Open(atlaspath, FileMode.Create);
                BinaryWriter binary = new BinaryWriter(file);
                binary.Write(bytes);
                file.Close();
                AssetDatabase.Refresh();
                //Create Material
                Material atlasMaterial = new Material(UnityEngine.Shader.Find(shadername));
                matpath = atlaspath.Replace(".png", ".mat");
                AssetDatabase.CreateAsset(atlasMaterial, matpath);
                atlasMaterial.SetTexture("_MainTex", (UnityEngine.Texture)AssetDatabase.LoadAssetAtPath(atlaspath, typeof(UnityEngine.Texture)));
                File.WriteAllLines(atlaspath.Replace(".png", "") + ".txt", coords);

                TextureImporter textureImporter = AssetImporter.GetAtPath(atlaspath) as TextureImporter;
                textureImporter.spriteImportMode = SpriteImportMode.None;
                AssetDatabase.ImportAsset(atlaspath);

                AssetDatabase.Refresh();
            }
        }

        public static void AtlasCoordinatesDocument()
        {
            Array.Resize(ref coords, specTextures.Count);
            for (int i = 0; i < specTextures.Count; i++)
            {
                coords[i] = ("*" + specTextures[i].name + "*" + rect[i]);
            }
        }


        //Get Between Method
        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }

        public static void RealUpdateUV(UnityEngine.GameObject go, string matname)
        {
            //Debug.Log("RealUpdateUV matname=" + matname);

            if (Names2Coord.ContainsKey(matname) == true)
            {
                float x = float.Parse(getBetween(Names2Coord[matname], "x", "y"));
                float y = float.Parse(getBetween(Names2Coord[matname], "y", "w"));
                float w = float.Parse(getBetween(Names2Coord[matname], "w", "h"));
                float h = float.Parse(getBetween(Names2Coord[matname], "h", "end"));

                //Debug.Log("matname="+matname+"Names2Coord[matname]="+Names2Coord[matname]);

                MeshFilter lMeshFilter = go.GetComponent<MeshFilter>();
                if (lMeshFilter != null)
                {
                    Mesh lMesh = XOMeshSplit.CreateMesh(lMeshFilter.sharedMesh, 0);
                    UnityEngine.Vector2[] _uvOffset = lMesh.uv;

                    //Debug.Log("matname=" + matname + " Length=" + _uvOffset.Length);

                    for (int uv = 0; uv < _uvOffset.Length; uv++)
                    {
                        //Debug.Log("before[uv].x = " + _uvOffset[uv].x + " [uv].y = " + _uvOffset[uv].y);
                        // if(_uvOffset[uv].x>1.0f) _uvOffset[uv].x = 1.0f;
                        // if(_uvOffset[uv].x<0.0f) _uvOffset[uv].x = 0.0f;

                        // if(_uvOffset[uv].y>1.0f) _uvOffset[uv].y = 1.0f;
                        // if(_uvOffset[uv].y<0.0f) _uvOffset[uv].y = 0.0f;

                        _uvOffset[uv] = new UnityEngine.Vector2(_uvOffset[uv].x * (w) + (x), _uvOffset[uv].y * (h) + (y));
                        //Debug.Log("after[uv].x = " + _uvOffset[uv].x + " [uv].y = " + _uvOffset[uv].y);
                    }

                    lMesh.uv = _uvOffset;
                    lMeshFilter.sharedMesh = lMesh;
                }
            }

            // float x = float.Parse (getBetween (_textureCoords[_textureIndex],"x","y"));
            // float y = float.Parse (getBetween (_textureCoords[_textureIndex],"y","w"));
            // float w = float.Parse (getBetween (_textureCoords[_textureIndex],"w","h"));
            // float h = float.Parse (getBetween (_textureCoords[_textureIndex],"h","end"));


            // Mesh _meshCopy = new Mesh ();

            // foreach(var property in typeof(Mesh).GetProperties()){
            //     if(property.GetSetMethod() != null && property.GetGetMethod() != null){ 
            //         property.SetValue(_meshCopy, property.GetValue(_goMesh[_meshIndex], null), null);
            //     }
            // }   
            // UnityEngine.Vector2[] _uvOffset = _meshCopy.uv;

            // for(int uv = 0; uv < _uvOffset.Length;uv++){
            //     _uvOffset[uv] = new UnityEngine.Vector2 (_uvOffset[uv].x * (w) + (x),_uvOffset[uv].y *(h)+(y));
            // }

            // _meshCopy.uv = _uvOffset;
            // _gos[_meshIndex].sharedMesh =_meshCopy;
        }

        public static void ReadSetup()
        {
            Names2Coord.Clear();
            string _texturePath = matpath.Replace(".mat", ".txt");
            StreamReader readAtlas = new StreamReader(_texturePath);
            string line;

            while ((line = readAtlas.ReadLine()) != null)
            {
                string data = getBetween(line, "*", "*");
                string dataX = getBetween(line, "x:", ",");
                string dataY = getBetween(line, "y:", ",");
                string dataW = getBetween(line, "width:", ",");
                string dataH = getBetween(line, "height:", ")");
                if (Names2Coord.ContainsKey(data) == false)
                {
                    Names2Coord.Add(data, "x" + dataX + "y" + dataY + "w" + dataW + "h" + dataH + "end");
                }
            }
            readAtlas.Close();
        }



        public static void CombineGameObjects(UnityEngine.GameObject[] gos)
        {
            if (gos != null)
            {
                MaterialMeshes lMaterialMeshes = new MaterialMeshes();

                for (int i = 0; i < gos.Length; i++)
                {
                    //UnityEngine.GameObject MaterialRoot = gos[i];
                    //for (int j = 0; j < MaterialRoot.transform.childCount; j++)
                    {
                        UnityEngine.GameObject go = gos[i];

                        MeshFilter lMeshFilter = go.GetComponent<MeshFilter>();
                        MeshRenderer lMeshRenderer = go.GetComponent<MeshRenderer>();
                        if (lMeshFilter && lMeshRenderer)
                        {
                            Mesh lMesh = lMeshFilter.sharedMesh;
                            Material lMaterial = lMeshRenderer.sharedMaterial;
                            if (lMesh && lMaterial)
                            {
                                lMaterialMeshes.Material = lMaterial;
                                lMaterialMeshes.Meshes.Add(lMesh);
                                lMaterialMeshes.MeshFilters.Add(lMeshFilter);
                                lMaterialMeshes.Transform.Add(lMeshFilter.transform.localToWorldMatrix);
                            }
                        }
                    }
                    //XOUtil.Display(MaterialRoot, false)
                    //MaterialRoot.SetActive(false);
                }
                MergeMeshes(lMaterialMeshes, XOGloble.Output);

                for (int i = 0; i < gos.Length; i++)
                {
                    {
                        UnityEngine.GameObject go = gos[i];

                        MeshFilter lMeshFilter = go.GetComponent<MeshFilter>();
                        MeshRenderer lMeshRenderer = go.GetComponent<MeshRenderer>();
                        if (lMeshFilter && lMeshRenderer)
                        {
                            Mesh lMesh = lMeshFilter.sharedMesh;
                            Material lMaterial = lMeshRenderer.sharedMaterial;
                            if (lMesh && lMaterial)
                            {
                                gos[i].SetActive(false);
                            }
                        }
                    }
                    //XOUtil.Display(MaterialRoot, false)
                    //MaterialRoot.SetActive(false);
                }

            }
        }



        public static void CombineMeshes(UnityEngine.GameObject[] gos)
        {
            if (gos != null)
            {
                MaterialMeshes lMaterialMeshes = new MaterialMeshes();

                for (int i = 0; i < gos.Length; i++)
                {
                    UnityEngine.GameObject MaterialRoot = gos[i];
                    for (int j = 0; j < MaterialRoot.transform.childCount; j++)
                    {
                        UnityEngine.GameObject go = MaterialRoot.transform.GetChild(j).gameObject;

                        MeshFilter lMeshFilter = go.GetComponent<MeshFilter>();
                        MeshRenderer lMeshRenderer = go.GetComponent<MeshRenderer>();
                        if (lMeshFilter && lMeshRenderer)
                        {
                            Mesh lMesh = lMeshFilter.sharedMesh;
                            Material lMaterial = lMeshRenderer.sharedMaterial;
                            if (lMesh && lMaterial)
                            {
                                lMaterialMeshes.Material = lMaterial;
                                lMaterialMeshes.Meshes.Add(lMesh);
                                lMaterialMeshes.MeshFilters.Add(lMeshFilter);
                                lMaterialMeshes.Transform.Add(lMeshFilter.transform.localToWorldMatrix);
                            }
                        }
                    }
                    //XOUtil.Display(MaterialRoot, false)
                    MaterialRoot.SetActive(false);
                }
                MergeMeshes(lMaterialMeshes, gos[0].transform.parent.gameObject);
            }
        }


        private static UnityEngine.GameObject MergeMeshes(MaterialMeshes rMaterialMeshes, UnityEngine.GameObject mMergedParent)
        {
            if (rMaterialMeshes == null) { return null; }
            if (rMaterialMeshes.Meshes.Count == 0) { return null; }

            int lMeshCount = rMaterialMeshes.Meshes.Count;

            // Determine the position of the new merged object
            UnityEngine.Vector3 lCenter = UnityEngine.Vector3.zero;
            for (int i = 0; i < lMeshCount; i++)
            {
                lCenter.x += rMaterialMeshes.Transform[i].m03;
                lCenter.y += rMaterialMeshes.Transform[i].m13;
                lCenter.z += rMaterialMeshes.Transform[i].m23;
            }

            lCenter /= lMeshCount;

            // Create the list of meshes that will be merged
            CombineInstance[] lMeshes = new CombineInstance[lMeshCount];
            for (int i = 0; i < lMeshCount; i++)
            {
                // Ensure our meshes are positioned relative to the center
                Matrix4x4 lMatrix = rMaterialMeshes.Transform[i];
                lMatrix.m03 -= lCenter.x;
                lMatrix.m13 -= lCenter.y;
                lMatrix.m23 -= lCenter.z;

                // Process sub meshes
                lMeshes[i].transform = lMatrix;
                lMeshes[i].mesh = rMaterialMeshes.Meshes[i];
            }

            // Create the object that will represent the new mesh
            UnityEngine.GameObject lMergedObject = new UnityEngine.GameObject();
            lMergedObject.name = "Merged Mesh:" + rMaterialMeshes.Meshes.Count;
            lMergedObject.transform.position = lCenter;

            // Combine the meshes in the new object
            MeshFilter lMergedMeshFilter = lMergedObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
            lMergedMeshFilter.mesh = new Mesh();
            lMergedMeshFilter.sharedMesh.name = "Merged Mesh of " + lMeshCount + " items";
            lMergedMeshFilter.sharedMesh.CombineMeshes(lMeshes, true);

            // Generate UV2 for lightmapping
            Unwrapping.GenerateSecondaryUVSet(lMergedMeshFilter.sharedMesh);

            // Set the material(s) for the new object
            MeshRenderer lMergedMeshRenderer = lMergedObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
            lMergedMeshRenderer.sharedMaterial = rMaterialMeshes.Material;

            // Assign the parent of the first selected object's parent
            if (mMergedParent != null)
            {
                lMergedObject.transform.parent = mMergedParent.transform;
            }

            lMergedObject.isStatic = true;
            XOCp_OptimizerHandler xoh = XOGloble.Root.GetComponent<XOCp_OptimizerHandler>();
            if (xoh != null)
            {
                xoh.nlst.Add(lMergedObject);
            }

            // Return the newly created game object
            return lMergedObject;
        }






    }
}

#endif

using UnityEngine;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using System.Collections.Generic;
public class pegar : MonoBehaviour
{
    // configure with raw, jpg, png, or ppm (simple raw format)
    public enum Format { RAW, JPG, PNG, PPM };
    public Format format = Format.PNG;

    // folder to write output (defaults to data path)
    public string folder;

    // private vars for screenshot
    private Texture2D screenShot;
    private int counter = 0; // image #

    // commands
    public GameObject oi;
    private WebCamTexture webcamTexture;
    Time tempo;
    //hhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhh
    //public Text ai;


    //[DllImport("ControleLibrary", EntryPoint = "keyrelease",
    //    CallingConvention = CallingConvention.StdCall)]
    //public static extern int keyrelease(string key);


    void Start()
    {
        
        WebCamDevice[] devices = WebCamTexture.devices;
        webcamTexture = new WebCamTexture();
        webcamTexture.requestedHeight = 600;
        webcamTexture.requestedWidth = 800;
        //webcamTexture.requestedFPS = 2;
        Renderer renderer = oi.GetComponent<Renderer>();
        renderer.material.mainTexture = webcamTexture;
        //webcamTexture.deviceName = devices[1].name;
		//Debug.Log (devices [0].name );
        webcamTexture.Play();
		//Debug.Log (devices [0].name + " , " + devices [1].name);
    }

    void Update()
    {
        //if (webcamTexture.didUpdateThisFrame)
        //{
            // Crie objetos de captura de tela, se necessário
            if (screenShot == null)
            {
                screenShot = new Texture2D(webcamTexture.width, webcamTexture.height, TextureFormat.RGB24, false);
            }
   
            screenShot.SetPixels(webcamTexture.GetPixels());

            // get our unique filename
            string filename = uniqueFilename((int)webcamTexture.width, (int)webcamTexture.height);

            //if (counter == 15)
            //{
                // Puxe nos bytes de cabeçalho / dados do arquivo para o formato de imagem especificado (deve ser feito a partir do segmento principal)
                //byte[] fileHeader = null;
                byte[] fileData = null;
                fileData = screenShot.EncodeToPNG();


                // create new thread to save the image to file (only operation that can be done in background)
                new System.Threading.Thread(() =>
                {
                    // create file and write optional header with image bytes
                     var f = System.IO.File.Create(filename);
                    //if (fileHeader != null) f.Write(fileHeader, 0, fileHeader.Length);
                    f.Write(fileData, 0, fileData.Length);
                    f.Close();
                    //Debug.Log(string.Format("No disco screenshot {0} of size {1}", filename, fileData.Length));
                    //--- classifica
                    
                }).Start();
            //}
        //}
    }
   

    private string uniqueFilename(int width, int height)
    {
        if (folder == null || folder.Length == 0)
        {
            //folder = Application.dataPath;
            /*if (Application.isEditor)
            {
                var stringPath = folder + "/..";
                folder = Path.GetFullPath(stringPath);
            }*/
			folder = "/imagens";
            
            // make sure directoroy exists
            System.IO.Directory.CreateDirectory(folder);

            // count number of files of specified format in folder
            string mask = string.Format("screen_{0}x{1}*.{2}", width, height, format.ToString().ToLower());
            counter = Directory.GetFiles(folder, mask, SearchOption.TopDirectoryOnly).Length;
        }

        // use width, height, and counter for unique file name
        var filename = string.Format("{0}/screen_{1}x{2}_{3}.{4}", folder, width, height, counter, format.ToString().ToLower());

        // up counter for next call
        ++counter;

       // if (counter > 30) counter = 1;

        // return unique filename
        return filename;
    }
}
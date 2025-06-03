// Using https://www.youtube.com/watch?v=ZV5eejYG6NI as Reference

using OpenCvSharp;
using OpenCvSharp.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ContourFinder : WebCamera
{
    // OpenCV Mat to hold the image data
    private Mat image;


    // HAVE TO INHERIT this class from WebCamera abstract
    // Class to get the WebCamTexture and process it
    protected override bool ProcessTexture(WebCamTexture input, ref Texture2D output)
    {
        image = OpenCvSharp.Unity.TextureToMat(input);

        //Processing Stuff

        if (output == null)
            output = OpenCvSharp.Unity.MatToTexture(image);
        else 
            OpenCvSharp.Unity.MatToTexture(image, output);

        return true; // Return true if the texture has been processed
    }
}

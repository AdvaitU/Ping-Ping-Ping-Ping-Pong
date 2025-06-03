using OpenCvSharp;
using OpenCvSharp.Demo;
using UnityEngine;

// With help from https://www.youtube.com/watch?v=ZV5eejYG6NI as Reference and help for Setup  + Some ChatGPT assistance

public class ContourFinder : WebCamera
{
    [Header("Color Detection Settings")]
    [SerializeField] private Color targetColorRGB = Color.blue; // Set your desired color in Unity Inspector
    [Range(1, 50)][SerializeField] private int hueRange = 10; // ± range for Hue
    [Range(30, 255)][SerializeField] private int minSaturation = 150;
    [Range(30, 255)][SerializeField] private int minValue = 50;
    [SerializeField] private float minContourArea = 500f;
    [SerializeField] private Scalar contourColor = new Scalar(0, 255, 0); // Green box for debug

    [Header("UI & Debug")]
    [SerializeField] private FlipMode ImageFlip;
    [SerializeField] private bool ShowMaskImage = true;

    private Mat image;
    private Mat hsvImage = new Mat();
    private Mat mask = new Mat();

    public Vector2 blobPosition = new();
    protected override bool ProcessTexture(WebCamTexture input, ref Texture2D output)
    {
        // 1. Convert input to Mat and flip
        image = OpenCvSharp.Unity.TextureToMat(input);
        Cv2.Flip(image, image, ImageFlip);

        // 2. Convert to HSV
        Cv2.CvtColor(image, hsvImage, ColorConversionCodes.BGR2HSV);

        // 3. Convert RGB color from Unity to HSV
        Color.RGBToHSV(targetColorRGB, out float h, out float s, out float v);
        int hue = Mathf.RoundToInt(h * 180f); // OpenCV Hue range is 0-180
        int sat = Mathf.RoundToInt(s * 255f);
        int val = Mathf.RoundToInt(v * 255f);

        // 4. Create HSV thresholds
        int lowerHue = Mathf.Clamp(hue - hueRange, 0, 180);
        int upperHue = Mathf.Clamp(hue + hueRange, 0, 180);

        Scalar lower = new Scalar(lowerHue, minSaturation, minValue);
        Scalar upper = new Scalar(upperHue, 255, 255);

        // 5. Threshold the HSV image
        Cv2.InRange(hsvImage, lower, upper, mask);

        // 6. Morphology cleanup
        Cv2.Erode(mask, mask, null, iterations: 1);
        Cv2.Dilate(mask, mask, null, iterations: 1);

        // 7. Find contours
        Point[][] contours;
        HierarchyIndex[] hierarchy;
        Cv2.FindContours(mask.Clone(), out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

        foreach (var contour in contours)
        {
            double area = Cv2.ContourArea(contour);
            if (area > minContourArea)
            {
                OpenCvSharp.Rect boundingBox = Cv2.BoundingRect(contour);
                Cv2.Rectangle(image, boundingBox, contourColor, 2);

                blobPosition = new Vector2(
                    (float)(boundingBox.X + boundingBox.Width / 2) / image.Width,
                    1f - (float)(boundingBox.Y + boundingBox.Height / 2) / image.Height
                );

                //Debug.Log($"Target color object detected at normalized position: {blobPosition}");
            }
        }

        // 8. Display result
        if (output == null)
            output = OpenCvSharp.Unity.MatToTexture(ShowMaskImage ? mask : image);
        else
            OpenCvSharp.Unity.MatToTexture(ShowMaskImage ? mask : image, output);

        return true;
    }
}

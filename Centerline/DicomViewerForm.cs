using Accord.Imaging.Converters;
using Accord.Imaging.Filters;
using DicomImageViewer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Centerline
{
    public partial class DicomViewerForm : Form
    {
        DicomDecoder dicomDecoder;
        List<byte> pixels8 { get { return dicomDecoder.pixels8; } }
        List<ushort> pixels16 { get { return dicomDecoder.pixels16; } }
        List<byte> pixels24 { get { return dicomDecoder.pixels24; } }
        List<List<ushort>> Cuts16;
        float scale;
        bool FirstPoint;
        bool SecondPoint;

        int cut;
        string fileName;
        public DicomViewerForm(string filename,float Scale)
        {
            cut = 0;
            scale = Scale;
            FirstPoint = false;
            SecondPoint = false;
            InitializeComponent();
            fileName = filename;
            dicomDecoder = new DicomDecoder();
            Cuts16 = new List<List<ushort>>();
        }

        private void DicomViewerForm_Load(object sender, EventArgs e)
        {
            int it = 2;
            while (true)
            {

                var file = fileName + $@"\I.{it.ToString("000")}.dcm";
                try
                {
                    dicomDecoder.DicomFileName = file;
                }
                catch (System.IO.FileNotFoundException)
                {
                    break; //Dejo de leer
                }
                Cuts16.Add(pixels16.ToList());
                it++;
            }
            Cut_label.Text = $@"0/{Cuts16.Count}";
            MatrixToImage conv = new MatrixToImage(min: 0, max: 1);
            //int N = 3;
            Bitmap image;
            conv.Convert(DicomVisualizer.Convert2DArray(Cuts16.First().ToArray(),512), out image);
            image = new ResizeNearestNeighbor(512, 512).Apply(image);
            pictureBox1.Image = image;
            pictureBox1.Size = new Size(512, 512);
            pictureBox1.MouseWheel += PictureBox1_MouseWheel;
        }

        private void PictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            //int M = 5;
            cut += e.Delta / 120;
            cut = cut > Cuts16.Count - 1 ? Cuts16.Count - 1 : cut < 0 ? 0 : cut;
            this.Invalidate();
            Cut_label.Text = $@"{cut}/{Cuts16.Count}";

            MatrixToImage conv = new MatrixToImage(min: 0, max: 1);
            //int N = 3;
            conv.Convert(DicomVisualizer.Convert2DArray(Cuts16[cut].ToArray(), 512), out Bitmap image);
            image = new ResizeNearestNeighbor(512, 512).Apply(image);
            pictureBox1.Image = image;
            pictureBox1.Size = new Size(512, 512);
        }

        private void SetFirstPoint_button_Click(object sender, EventArgs e)
        {
            pictureBox1.Cursor = System.Windows.Forms.Cursors.Cross;
            FirstPoint = true;
        }



        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (FirstPoint)
                DicomVisualizer.PathFindingStartPoint = new Solver.Vector3D((int)((cut + 1) * scale)*0.1f, (int)((512-e.Y+1)*scale)*0.1f, (int)((512 - e.X + 1) * scale)*0.1f);
            if (SecondPoint)
                DicomVisualizer.PathFindingEndPoint = new Solver.Vector3D((int)((cut + 1) * scale)*0.1f, (int)((512-e.Y+1)*scale) * 0.1f, (int)((512 - e.X + 1) * scale) * 0.1f); 
            pictureBox1.Cursor = System.Windows.Forms.Cursors.Default;
            FirstPoint = false;
            SecondPoint = false;
        }
        //var PathFindingEndPointTemp = new Vector3D((512 - 319 + 1) * scale, (512 - 471 + 1) * scale, (51 + 1) * scale);//165,247,29
        //PathFindingEndPoint = new Vector3D((int)PathFindingEndPointTemp.Z*0.1f, (int)PathFindingEndPointTemp.Y * 0.1f,(int)PathFindingEndPointTemp.X * 0.1f);
        private void SetSecondPoint_button_Click(object sender, EventArgs e)
        {
            pictureBox1.Cursor = System.Windows.Forms.Cursors.Cross;
            SecondPoint = true;
        }

        private void Done_button_Click(object sender, EventArgs e)
        {
            if (DicomVisualizer.PathFindingStartPoint == null || DicomVisualizer.PathFindingEndPoint == null)
                MessageBox.Show("Please select both points", "Invalid set!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
                this.Close();
        }
    }
}

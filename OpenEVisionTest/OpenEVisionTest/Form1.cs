using Euresys.Open_eVision_2_2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenEVisionTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        OpenFileDialog OpenSFile = new OpenFileDialog();
        OpenFileDialog OpenS2File = new OpenFileDialog();
        OpenFileDialog OpenDFile = new OpenFileDialog();

        EMatcher EMatcher1 = new EMatcher(); // EMatcher instance
        EImageBW8 EBW8Image1 = new EImageBW8(); // EImageBW8 instance
        EImageBW8 EBW8Image2 = new EImageBW8(); // EImageBW8 instance

        //Test
        EImageBW16 EBW16Image1 = new EImageBW16();
        EImageBW16 EBW16Image2 = new EImageBW16();

        int SN = 0;
        int SN_T = 0;
        int SN_C = 0;
        int SN_CC = 0;
        int SN_SR = 0;
        int SN_ME = 0;

        //相似度
        private void buttonSource_Click(object sender, EventArgs e)
        {           
            DialogResult result = OpenSFile.ShowDialog();

            if(result == DialogResult.OK)
            {
                this.pictureBoxSource.Image = Image.FromFile(OpenSFile.FileName);
            }
        }

        private void buttonDestination_Click(object sender, EventArgs e)
        {           
            DialogResult result = OpenDFile.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.pictureBoxDestination.Image = Image.FromFile(OpenDFile.FileName);
            }
        }

        private void buttonExecute_Click(object sender, EventArgs e)
        {
            try
            {
                EBW8Image1.Load(OpenSFile.FileName);                
                EMatcher1.MaxScale = 1.00f;
                EMatcher1.MinScale = 1.00f;
                EMatcher1.LearnPattern(EBW8Image1);
                SN++;
                EMatcher1.Save($"D:\\Ryan_Chiu\\OPFile\\Compare\\{SN}.MCH");
                EBW8Image2.Load(OpenDFile.FileName);
                EMatcher1.Match(EBW8Image2);

                // Retrieve the number of occurrences
                int numOccurrences = EMatcher1.NumPositions;
                // Retrieve the first occurrence
                EMatchPosition myOccurrence = EMatcher1.GetPosition(0);
                // Retrieve its score and position
                float score = myOccurrence.Score;
                //float centerX = myOccurrence.CenterX;
                //float centerY = myOccurrence.CenterY;
                MessageBox.Show("相似度 : " + score.ToString("P"));
                
            }
            catch (EException)
            {
                // Insert exception handling code here
            }
            
        }

        //根據Model檔來匹配樣本圖
        private void buttonModel_Click(object sender, EventArgs e)
        {
            DialogResult result = OpenSFile.ShowDialog();

            if (result == DialogResult.OK)
            {
                EMatcher1.Load(OpenSFile.FileName);
            }
        }

        private void buttonSourceMatch_Click(object sender, EventArgs e)
        {
            DialogResult result = OpenS2File.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.pictureBoxSourceMatch.Image = Image.FromFile(OpenS2File.FileName);
            }
        }

        private void buttonExecuteMatch_Click(object sender, EventArgs e)
        {
            EMatcher1.MaxAngle = 180.00f;
            EMatcher1.MinAngle = -180.00f;
            EMatcher1.MaxPositions = 4;
            EMatcher1.MinScore = 0.95f;
            EBW8Image1.Load(OpenS2File.FileName);
            EMatcher1.Match(EBW8Image1);
            EMatchPosition pos;
            
            try
            {
                for (int i = 0; i < EMatcher1.NumPositions; i++)
                {
                    pos = EMatcher1.GetPosition(i);
                    MessageBox.Show(i + " : (X : " + pos.CenterX + "，Y : " + pos.CenterY + ") 角度 : " + pos.Angle + "，相似度 : " + pos.Score.ToString("P"));
                }
            }
            catch (EException)
            {
                // Insert exception handling code here
            }
        }

        //Match Roi
        private void buttonSourceRoi_Click(object sender, EventArgs e)
        {
            DialogResult result = OpenSFile.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.pictureBoxMatchRoi.Image = Image.FromFile(OpenSFile.FileName);
            }
        }

        private void buttonExecuteRoi_Click(object sender, EventArgs e)
        {
            EImageC24 EC24Image1 = new EImageC24(); // EImageC24 instance
            EROIC24 EC24Image1Roi1 = new EROIC24(); // EROIC24 instance
            EMatcher EMatcher1 = new EMatcher(); // EMatcher instance

            try
            {
                EC24Image1.Load(OpenSFile.FileName);
                // Attach the roi to its parent
                EC24Image1Roi1.Attach(EC24Image1);
                EMatcher1.MaxScale = 1.00f;
                EMatcher1.MinScale = 1.00f;
                EC24Image1Roi1.SetPlacement(84, 372, 172, 56);
                EMatcher1.LearnPattern(EC24Image1Roi1);
                EMatcher1.MaxPositions = 2;
                EMatcher1.MinScore = 0.7f;
                EMatcher1.Match(EC24Image1);
                EMatchPosition pos;

                for (int i = 0; i < EMatcher1.NumPositions; i++)
                {
                    pos = EMatcher1.GetPosition(i);
                    MessageBox.Show(i + " : (X : " + pos.CenterX + "，Y : " + pos.CenterY + ") 角度 : " + pos.Angle + "，相似度 : " + pos.Score.ToString("P"));
                }

            }
            catch (EException)
            {
                // Insert exception handling code here
            }

        }

        //Don’t care area
        private void buttonSourceDtCare_Click(object sender, EventArgs e)
        {
            DialogResult result = OpenSFile.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.pictureBoxSourceDtCare.Image = Image.FromFile(OpenSFile.FileName);
            }
        }

        private void buttonSourceDtCare2_Click(object sender, EventArgs e)
        {
            DialogResult result = OpenS2File.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.pictureBoxSourceDtCare2.Image = Image.FromFile(OpenS2File.FileName);
            }
        }

        private void buttonExecuteDtCare_Click(object sender, EventArgs e)
        {
            EMatcher EMatcher1 = new EMatcher(); // EMatcher instance
            EImageBW8 EBW8Image1 = new EImageBW8(); // EImageBW8 instance
            EImageBW8 EBW8Image2 = new EImageBW8(); // EImageBW8 instance

            try
            {
                EBW8Image1.Load(OpenSFile.FileName);
                EMatcher1.MaxScale = 1.00f;
                EMatcher1.MinScale = 1.00f;
                EMatcher1.LearnPattern(EBW8Image1);
                EBW8Image2.Load(OpenS2File.FileName);
                EMatcher1.Interpolate = true;
                EMatcher1.MaxAngle = 10.00f;
                EMatcher1.MaxPositions = 3;
                EMatcher1.MinScore = 0.7f;
                EMatcher1.MinAngle = -10.00f;
                EMatcher1.Match(EBW8Image2);
                EMatcher1.DontCareThreshold = 1;
                EMatcher1.LearnPattern(EBW8Image1);
                EMatcher1.Match(EBW8Image2);

                EMatchPosition pos;
                for (int i = 0; i < EMatcher1.NumPositions; i++)
                {
                    pos = EMatcher1.GetPosition(i);
                    MessageBox.Show(i + " : (X : " + pos.CenterX + "，Y : " + pos.CenterY + ") 角度 : " + pos.Angle + "，相似度 : " + pos.Score.ToString("P"));
                }
            }
            catch (EException)
            {
                // Insert exception handling code here
            }

        }

        //增強圖像(卷積)
        private void buttonSourceConvolution_Click(object sender, EventArgs e)
        {
            DialogResult result = OpenSFile.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.pictureBoxConvolution.Image = Image.FromFile(OpenSFile.FileName);
            }
        }

        private void buttonExecuteConvolution_Click(object sender, EventArgs e)
        {
            EImageBW8 EBW8Image1 = new EImageBW8(); // EImageBW8 instance
            EImageBW8 EBW8Image2 = new EImageBW8(); // EImageBW8 instance
            EKernel EKernel1 = new EKernel(); // EKernel instance

            try
            {
                EBW8Image1.Load(OpenSFile.FileName);
                EBW8Image2.SetSize(512, 512);
                // Make image black
                EasyImage.Oper(EArithmeticLogicOperation.Copy, new EBW8(0), EBW8Image2);
                EBW8Image2.SetSize(EBW8Image1);
                EasyImage.ConvolHighpass2(EBW8Image1, EBW8Image2);
                //EKernel1.SizeX = 3;
                //EKernel1.SizeY = 3;
                EKernel1.SetSize(3, 3);
                EKernel1.SetKernelData(0, 0, -1.00f);
                EKernel1.SetKernelData(1, 0, -1.00f);
                EKernel1.SetKernelData(2, 0, -1.00f);
                EKernel1.SetKernelData(0, 1, -1.00f);
                EKernel1.SetKernelData(1, 1, 15.00f);
                EKernel1.SetKernelData(2, 1, -1.00f);
                EKernel1.SetKernelData(0, 2, -1.00f);
                EKernel1.SetKernelData(1, 2, -1.00f);
                EKernel1.SetKernelData(2, 2, -1.00f);
                EasyImage.ConvolKernel(EBW8Image1, EBW8Image2, EKernel1);

                EBW8Image2.Save($"D:\\Ryan_Chiu\\OPFile\\Convolution\\{SN_C}.jpeg");
                MessageBox.Show("OK");
            }
            catch (EException)
            {
                // Insert exception handling code here
            }

        }

        //自訂的彈性遮罩
        private void buttonSourceT_Click(object sender, EventArgs e)
        {
            DialogResult result = OpenSFile.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.pictureBoxSourceT.Image = Image.FromFile(OpenSFile.FileName);
            }
            
        }

        private void buttonExecuteT_Click(object sender, EventArgs e)
        {
            //二值化
            //try
            //{
            //    EBW16Image1.Load(OpenSFile.FileName);
            //    EBW16Image2.SetSize(512, 512);
            //    // Make image black
            //    EasyImage.Oper(EArithmeticLogicOperation.Copy, new EBW16(0), EBW16Image2);
            //    var m = (int)EThresholdMode.MinResidue;                
            //    EasyImage.Threshold(EBW16Image1, EBW16Image2, m);                
            //}
            //catch (EException)
            //{
            //    // Insert exception handling code here
            //}
            
            EImageBW8 EBW8Image3 = new EImageBW8(); // EImageBW8 instance
            EImageBW8 EBW8Image4 = new EImageBW8(); // EImageBW8 instance
            EImageBW8 EBW8Image5 = new EImageBW8(); // EImageBW8 instance

            try
            {
                EBW8Image3.Load(OpenSFile.FileName);
                EBW8Image4.SetSize(512, 512);
                // Make image black
                EasyImage.Oper(EArithmeticLogicOperation.Copy, new EBW8(0), EBW8Image4);
                EBW8Image4.SetSize(EBW8Image3);
                EasyImage.Oper(EArithmeticLogicOperation.Invert, EBW8Image3, EBW8Image4);
                EBW8Image5.SetSize(512, 512);
                // Make image black
                EasyImage.Oper(EArithmeticLogicOperation.Copy, new EBW8(0), EBW8Image5);
                EBW8Image5.SetSize(EBW8Image4);
                EasyImage.Threshold(EBW8Image4, EBW8Image5, 46);
                SN_T++;
                EBW8Image4.SaveJpeg($"D:\\Ryan_Chiu\\OPFile\\Threshold\\{SN_T}.jpeg", 75);
                MessageBox.Show("OK");
            }
            catch (EException)
            {
                // Insert exception handling code here
            }
        }

        //Color Conversion
        private void buttonSourceColor_Click(object sender, EventArgs e)
        {
            DialogResult result = OpenSFile.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.pictureBoxColor.Image = Image.FromFile(OpenSFile.FileName);
            }
        }

        private void buttonExecuteColor_Click(object sender, EventArgs e)
        {
            EImageC24 EC24Image7 = new EImageC24(); // EImageC24 instance
            EImageBW8 EBW8Image8 = new EImageBW8(); // EImageBW8 instance

            try
            {
                EC24Image7.Load(OpenSFile.FileName);
                EBW8Image8.SetSize(512, 512);
                // Make image black
                EasyImage.Oper(EArithmeticLogicOperation.Copy, new EBW8(0), EBW8Image8);
                EBW8Image8.SetSize(EC24Image7);
                EasyImage.Convert(EC24Image7, EBW8Image8);

                EBW8Image8.Save($"D:\\Ryan_Chiu\\OPFile\\ColorConversion\\{SN_CC}.jpeg");
                MessageBox.Show("OK");
            }
            catch (EException)
            {
                // Insert exception handling code here
            }
        }

        //圖像分割後移除不重要的物體
        private void buttonSourceS_Click(object sender, EventArgs e)
        {
            DialogResult result = OpenSFile.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.pictureBoxSourceS.Image = Image.FromFile(OpenSFile.FileName);
            }
        }
        
        private void buttonExecuteS_Click(object sender, EventArgs e)
        {            
            ECodedImage2 codedImage1 = new ECodedImage2(); // ECodedImage2 instance
            EImageEncoder codedImage1Encoder = new EImageEncoder(); // EImageEncoder instance
            EObjectSelection codedImage1ObjectSelection = new EObjectSelection(); // EObjectSelection instance
            EImageBW8 EBW8Image6 = new EImageBW8(); // EImageBW8 instance

            try
            {
                codedImage1ObjectSelection.FeretAngle = 0.00f;
                EBW8Image6.Load(OpenSFile.FileName);
                codedImage1Encoder.GrayscaleSingleThresholdSegmenter.BlackLayerEncoded = true;
                codedImage1Encoder.GrayscaleSingleThresholdSegmenter.WhiteLayerEncoded = false;
                //test
                codedImage1Encoder.SegmentationMethod = ESegmentationMethod.GrayscaleSingleThreshold;
                //test
                codedImage1Encoder.GrayscaleSingleThresholdSegmenter.Mode = EGrayscaleSingleThreshold.Absolute;
                codedImage1Encoder.GrayscaleSingleThresholdSegmenter.AbsoluteThreshold = 115;
                codedImage1Encoder.Encode(EBW8Image6, codedImage1);
                codedImage1ObjectSelection.Clear();
                codedImage1ObjectSelection.AddObjects(codedImage1);
                codedImage1ObjectSelection.AttachedImage = EBW8Image6;
                codedImage1ObjectSelection.RemoveUsingUnsignedIntegerFeature(EFeature.Area, 100, ESingleThresholdMode.Less);

                EBW8Image6.Save($"D:\\Ryan_Chiu\\OPFile\\Remove\\{SN_SR}.jpeg");
                var c = codedImage1ObjectSelection.ElementCount;
                MessageBox.Show("取得物件數 : " + c);

            }
            catch (EException)
            {
                // Insert exception handling code here
            }
        }

        //遮罩檢測錯誤
        private void buttonSourceMaskError_Click(object sender, EventArgs e)
        {
            DialogResult result = OpenSFile.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.pictureBoxSourceMaskError.Image = Image.FromFile(OpenSFile.FileName);
            }
        }

        private void buttonMaskMaskError_Click(object sender, EventArgs e)
        {
            DialogResult result = OpenS2File.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.pictureBoxMaskMaskError.Image = Image.FromFile(OpenS2File.FileName);
            }
        }

        private void buttonExecuteMaskError_Click(object sender, EventArgs e)
        {
            ECodedImage2 codedImage1 = new ECodedImage2(); // ECodedImage2 instance
            EImageEncoder codedImage1Encoder = new EImageEncoder(); // EImageEncoder instance
            EObjectSelection codedImage1ObjectSelection = new EObjectSelection(); // EObjectSelection instance
            EImageBW8 EBW8Image1 = new EImageBW8(); // EImageBW8 instance
            EImageBW8 mask = new EImageBW8(); // EImageBW8 instance

            try
            {
                codedImage1ObjectSelection.FeretAngle = 0.00f;
                EBW8Image1.Load(OpenSFile.FileName);
                mask.Load(OpenS2File.FileName);
                codedImage1Encoder.GrayscaleSingleThresholdSegmenter.Mode = EGrayscaleSingleThreshold.Absolute;
                codedImage1Encoder.GrayscaleSingleThresholdSegmenter.AbsoluteThreshold = 202;
                codedImage1Encoder.Encode(EBW8Image1, mask, codedImage1);
                codedImage1ObjectSelection.Clear();
                codedImage1ObjectSelection.AddObjects(codedImage1);
                codedImage1ObjectSelection.AttachedImage = EBW8Image1;
                
                SN_ME++;
                EBW8Image1.Save($"D:\\Ryan_Chiu\\OPFile\\MaskError\\{SN_ME}.jpeg");

                var c = codedImage1.GetObjCount();
                MessageBox.Show("取得物件數 : " + c.ToString());
            }
            catch (EException)
            {
                // Insert exception handling code here
            }

        }

        //最小 - 最大參考檢測圖像之間的差異
        private void buttonSourceMinMax_Click(object sender, EventArgs e)
        {
            DialogResult result = OpenSFile.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.pictureBoxMinMax.Image = Image.FromFile(OpenSFile.FileName);
            }
        }

        private void buttonSourceMinMax2_Click(object sender, EventArgs e)
        {
            DialogResult result = OpenS2File.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.pictureBoxMinMax2.Image = Image.FromFile(OpenS2File.FileName);
            }
        }

        private void buttonExecuteMinMax_Click(object sender, EventArgs e)
        {
            EImageBW8 filmOk = new EImageBW8(); // EImageBW8 instance
            EImageBW8 filmOkMin = new EImageBW8(); // EImageBW8 instance
            EImageBW8 filmOkMax = new EImageBW8(); // EImageBW8 instance
            //EROIBW1 filmOk = new EROIBW1();
            //EROIBW1 filmOkMin = new EROIBW1();
            //EROIBW1 filmOkMax = new EROIBW1(); 
            ECodedImage2 codedImage1 = new ECodedImage2(); // ECodedImage2 instance
            EImageEncoder codedImage1Encoder = new EImageEncoder(); // EImageEncoder instance
            EObjectSelection codedImage1ObjectSelection = new EObjectSelection(); // EObjectSelection instance
            EImageBW8 filmBad = new EImageBW8(); // EImageBW8 instance

            try
            {
                filmOk.Load(OpenSFile.FileName);
                filmOkMin.SetSize(768, 576);
                // Make image black
                EasyImage.Oper(EArithmeticLogicOperation.Copy, new EBW8(0), filmOkMin);
                filmOkMax.SetSize(768, 576);
                // Make image black
                EasyImage.Oper(EArithmeticLogicOperation.Copy, new EBW8(0), filmOkMax);
                filmOkMin.SetSize(filmOk);
                filmOkMax.SetSize(filmOk);
                EasyImage.ErodeDisk(filmOk, filmOkMin, 1);
                EasyImage.Oper(EArithmeticLogicOperation.Subtract, filmOkMin, new EBW8(10), filmOkMin);
                EasyImage.DilateDisk(filmOk, filmOkMax, 1);
                EasyImage.Oper(EArithmeticLogicOperation.Add, filmOkMax, new EBW8(10), filmOkMax);
                codedImage1ObjectSelection.FeretAngle = 0.00f;
                filmBad.Load(OpenS2File.FileName);
                codedImage1Encoder.SegmentationMethod = ESegmentationMethod.ImageRange;
                codedImage1Encoder.ImageRangeSegmenter.WhiteLayerEncoded = false;
                codedImage1Encoder.ImageRangeSegmenter.BlackLayerEncoded = true;
                //codedImage1Encoder.ImageRangeSegmenter.HighImage = filmOkMax;
                //codedImage1Encoder.ImageRangeSegmenter.LowImage = filmOkMin;
                codedImage1Encoder.ImageRangeSegmenter.HighImageBW8 = filmOkMax;
                codedImage1Encoder.ImageRangeSegmenter.LowImageBW8 = filmOkMin;
                codedImage1Encoder.Encode(filmBad, codedImage1);
                codedImage1ObjectSelection.Clear();
                codedImage1ObjectSelection.AddObjects(codedImage1);
                codedImage1ObjectSelection.AttachedImage = filmBad;

                var lc = codedImage1.LayerCount;
                for (int i = 0; i <= lc; i++)
                {
                    MessageBox.Show($"第{i}層有" + codedImage1.GetObjCount(i).ToString() + "個物件");
                }
            }
            catch (EException)
            {
                // Insert exception handling code here
            }
        }

        //Line Gauge(量測旋轉角度)
        private void buttonSourceR_Click(object sender, EventArgs e)
        {
            DialogResult result = OpenSFile.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.pictureBoxSourceR.Image = Image.FromFile(OpenSFile.FileName);
            }
        }
        
        private void buttonExecuteR_Click(object sender, EventArgs e)
        {
            EWorldShape EWorldShape1 = new EWorldShape(); // EWorldShape instance
            EImageBW8 EBW8Image7 = new EImageBW8(); // EImageBW8 instance
            ELineGauge ELineGauge1 = new ELineGauge(); // ELineGauge instance
            ELine measuredLine = null; // ELine instance

            try
            {
                EBW8Image7.Load(OpenSFile.FileName);
                EWorldShape1.SetSensorSize(768, 576);
                EWorldShape1.Process(EBW8Image7, true);
                ELineGauge1.Attach(EWorldShape1);
                ELineGauge1.Dragable = true;
                ELineGauge1.Resizable = true;
                ELineGauge1.Rotatable = true;
                ELineGauge1.TransitionType = ETransitionType.Wb;
                ELineGauge1.Measure(EBW8Image7);
                measuredLine = ELineGauge1.MeasuredLine;
                EWorldShape1.Process(EBW8Image7, true);

                var angle = ELineGauge1.Angle;
                MessageBox.Show("角度值 : " + angle.ToString());

            }
            catch (EException)
            {
                // Insert exception handling code here
            }
        }

        //Circle Gauge
        private void buttonSourceCircle1_Click(object sender, EventArgs e)
        {
            DialogResult result = OpenSFile.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.pictureBoxCircle1.Image = Image.FromFile(OpenSFile.FileName);
            }
        }

        private void buttonSourceCircle2_Click(object sender, EventArgs e)
        {
            DialogResult result = OpenS2File.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.pictureBoxCircle2.Image = Image.FromFile(OpenS2File.FileName);
            }
        }

        private void buttonExecuteCircle_Click(object sender, EventArgs e)
        {
            EWorldShape EWorldShape1 = new EWorldShape(); // EWorldShape instance
            EImageBW8 EBW8Image1 = new EImageBW8(); // EImageBW8 instance
            EImageBW8 EBW8Image2 = new EImageBW8(); // EImageBW8 instance
            ECircleGauge ECircleGauge1 = new ECircleGauge(); // ECircleGauge instance
            ECircle Circle1 = new ECircle(); // ECircle instance
            ECircle measuredCircle = null; // ECircle instance

            try
            {
                EBW8Image1.Load(OpenSFile.FileName);
                EWorldShape1.Process(EBW8Image1, true);
                EWorldShape1.AutoCalibrateDotGrid(EBW8Image1, 5.00f, 5.00f, false);
                EBW8Image2.Load(OpenS2File.FileName);
                EWorldShape1.SetSensorSize(768, 576);
                EWorldShape1.Process(EBW8Image2, true);
                ECircleGauge1.Attach(EWorldShape1);
                ECircleGauge1.Dragable = true;
                ECircleGauge1.Resizable = true;
                ECircleGauge1.Rotatable = true;
                ECircleGauge1.TransitionChoice = ETransitionChoice.NthFromBegin;
                ECircleGauge1.Measure(EBW8Image2);
                measuredCircle = ECircleGauge1.MeasuredCircle;
                EWorldShape1.Process(EBW8Image2, true);

                var d = ECircleGauge1.Diameter;
                MessageBox.Show("直徑 : " + d.ToString());                
            }
            catch (EException)
            {
                // Insert exception handling code here
            }

        }

        //EasyMatrixCode Model檔
        private void buttonSourceM_Click(object sender, EventArgs e)
        {
            DialogResult result = OpenSFile.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.pictureBoxSourceM.Image = Image.FromFile(OpenSFile.FileName);
            }
        }

        private void buttonSourceM2_Click(object sender, EventArgs e)
        {
            
            DialogResult result = OpenS2File.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.pictureBoxSourceM2.Image = Image.FromFile(OpenS2File.FileName);
            }
        }
        
        private void buttonExecuteM_Click(object sender, EventArgs e)
        {
            EMatrixCodeReader EMatrixCodeReader1 = new EMatrixCodeReader(); // EMatrixCodeReader instance
            EMatrixCode EMatrixCodeReader1Result = null; // EMatrixCode instance
            EImageBW8 EBW8Image8 = new EImageBW8(); // EImageBW8 instance
            EImageBW8 EBW8Image9 = new EImageBW8(); // EImageBW8 instance

            try
            {
                EBW8Image8.Load(OpenSFile.FileName);
                EMatrixCodeReader1.TimeOut = 5000000;
                EMatrixCodeReader1Result = EMatrixCodeReader1.Learn(EBW8Image8);
                EMatrixCodeReader1.Save("D:\\Ryan_Chiu\\OPFile\\Matrix\\MatrixModel.MX2");
                EMatrixCodeReader1Result = EMatrixCodeReader1.Read(EBW8Image8);
                EMatrixCodeReader1Result = EMatrixCodeReader1.Read(EBW8Image8);
                EMatrixCodeReader1Result = EMatrixCodeReader1.Read(EBW8Image8);
                EMatrixCodeReader1Result = EMatrixCodeReader1.Read(EBW8Image8);
                EBW8Image9.Load(OpenS2File.FileName);
                EMatrixCodeReader1Result = EMatrixCodeReader1.LearnMore(EBW8Image9);
                EMatrixCodeReader1.Save("D:\\Ryan_Chiu\\OPFile\\Matrix\\MatrixModel.MX2");
                MessageBox.Show("OK");
            }
            catch (EException)
            {
                // Insert exception handling code here
            }
        }

        //讀二維條碼
        private void buttonSourceRM_Click(object sender, EventArgs e)
        {
            DialogResult result = OpenSFile.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.pictureBoxSourceRM.Image = Image.FromFile(OpenSFile.FileName);
            }
        }

        private void buttonExecuteRM_Click(object sender, EventArgs e)
        {
            EMatrixCodeReader EMatrixCodeReader1 = new EMatrixCodeReader(); // EMatrixCodeReader instance
            EMatrixCode EMatrixCodeReader1Result = null; // EMatrixCode instance
            EImageBW8 EBW8Image1 = new EImageBW8(); // EImageBW8 instance
            
            try
            {
                EBW8Image1.Load(OpenSFile.FileName);
                EMatrixCodeReader1Result = EMatrixCodeReader1.Read(EBW8Image1);
                MessageBox.Show("Results Code : " + EMatrixCodeReader1Result.DecodedString);
            }
            catch (EException)
            {
                // Insert exception handling code here
            }
        }

        //Gain/Offset
        private void buttonSourceGainOffset_Click(object sender, EventArgs e)
        {
            DialogResult result = OpenSFile.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.pictureBoxGainOffset.Image = Image.FromFile(OpenSFile.FileName);
            }
        }

        private void buttonExecuteGainOffset_Click(object sender, EventArgs e)
        {
            EImageBW8 EBW8Image1 = new EImageBW8(); // EImageBW8 instance
            EImageBW8 EBW8Image2 = new EImageBW8(); // EImageBW8 instance

            try
            {
                EBW8Image1.Load(OpenSFile.FileName);
                EBW8Image2.SetSize(512, 512);
                // Make image black
                EasyImage.Oper(EArithmeticLogicOperation.Copy, new EBW8(0), EBW8Image2);
                EBW8Image2.SetSize(EBW8Image1);
                EasyImage.GainOffset(EBW8Image1, EBW8Image2, 1.50f, -170.00f);

                EBW8Image2.Save("D:\\Ryan_Chiu\\OPFile\\Test\\GainOffset.jpeg");
                MessageBox.Show("OK");
            }
            catch (EException)
            {
                // Insert exception handling code here
            }

        }

        //Morphology (Gradient梯度)
        private void buttonSourceMorphology_Click(object sender, EventArgs e)
        {
            DialogResult result = OpenSFile.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.pictureBoxMorphology.Image = Image.FromFile(OpenSFile.FileName);
            }
        }

        private void buttonExecuteMorphology_Click(object sender, EventArgs e)
        {
            EImageBW8 EBW8Image3 = new EImageBW8(); // EImageBW8 instance
            EImageBW8 EBW8Image4 = new EImageBW8(); // EImageBW8 instance
            
            try
            {
                
                EBW8Image3.Load(OpenSFile.FileName);
                EBW8Image4.SetSize(512, 512);
                // Make image black
                EasyImage.Oper(EArithmeticLogicOperation.Copy, new EBW8(0), EBW8Image4);
                EBW8Image4.SetSize(EBW8Image3);                
                EasyImage.MorphoGradientBox(EBW8Image3, EBW8Image4, 1);

                EBW8Image4.Save("D:\\Ryan_Chiu\\OPFile\\Test\\Morphology.jpeg");
                MessageBox.Show("OK");
            }
            catch (EException)
            {
                // Insert exception handling code here
            }

        }

        //Histogram
        private void buttonSourceHistogram_Click(object sender, EventArgs e)
        {
            DialogResult result = OpenSFile.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.pictureBoxHistogram.Image = Image.FromFile(OpenSFile.FileName);
            }
        }

        private void buttonExecuteHistogram_Click(object sender, EventArgs e)
        {            
            
            EImageBW8 EBW8Image6 = new EImageBW8(); // EImageBW8 instance
            EImageBW8 EBW8Image7 = new EImageBW8(); // EImageBW8 instance

            try
            {
                
                EBW8Image6.Load(OpenSFile.FileName);
                EBW8Image7.SetSize(512, 512);
                // Make image black
                EasyImage.Oper(EArithmeticLogicOperation.Copy, new EBW8(0), EBW8Image7);
                EBW8Image7.SetSize(EBW8Image6);
                EasyImage.Equalize(EBW8Image6, EBW8Image7);

                EBW8Image7.Save("D:\\Ryan_Chiu\\OPFile\\Test\\Histogram.jpeg");
                MessageBox.Show("OK");
            }
            catch (EException)
            {
                // Insert exception handling code here
            }

        }
    }
}

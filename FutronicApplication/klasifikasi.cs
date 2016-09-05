using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SourceAFIS.Simple;
using SourceAFIS.Extraction;
using SourceAFIS.General;
using SourceAFIS.Extraction.Filters;
using SourceAFIS.Extraction.Model;

#if !COMPACT_FRAMEWORK

#endif

namespace FutronicApplication
{
    class klasifikasi
    {
        #region propertis
        static AfisEngine Afis = new AfisEngine();
        static Extractor ekstrak = new Extractor();
        public DetailLogger.Hook Logger = DetailLogger.Null;
        public LocalHistogram Histogram = new LocalHistogram();
        public int BlockSize = 15;
        public Equalizer Equalizer = new Equalizer();
        public SegmentationMask Mask = new SegmentationMask();
        public HillOrientation Orientation = new HillOrientation();
        public OrientedSmoother RidgeSmoother = new OrientedSmoother();
        public OrientedSmoother OrthogonalSmoother = new OrientedSmoother();
        public ThresholdBinarizer Binarizer = new ThresholdBinarizer();
        public VotingFilter BinarySmoother = new VotingFilter();
        public CrossRemover CrossRemover = new CrossRemover();
        public InnerMask InnerMask = new InnerMask();
        public Thinner Thinner = new Thinner();
        public RidgeTracer RidgeTracer = new RidgeTracer();
        public DotRemover DotRemover = new DotRemover();
        public PoreRemover PoreRemover = new PoreRemover();
        public GapRemover GapRemover = new GapRemover();
        public TailRemover TailRemover = new TailRemover();
        public FragmentRemover FragmentRemover = new FragmentRemover();
        public BranchMinutiaRemover BranchMinutiaRemover = new BranchMinutiaRemover();
        #endregion

        #region kernel
        public byte[,] templ0 = new byte[15, 15] { 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            };//template 0
        public byte[,] templ22 = new byte[15, 15] { 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 0, 0, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 0, 0, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 0, 0, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 0, 0, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 0, 0, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 0, 0, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            };//template 22
        public byte[,] templ45 = new byte[15, 15] { 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 0, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 0, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 0, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 0, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            };//template 45
        public byte[,] templ67 = new byte[15, 15] { 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 0, 225, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            };//template 67
        public byte[,] templ90 = new byte[15, 15] { 
            {  255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255 }, 
            };//template 90
        public byte[,] templ112 = new byte[15, 15] { 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 0, 225, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            };//template 112
        public byte[,] templ135 = new byte[15, 15] { 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 0, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 0, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 0, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 0, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            };//template 135
        public byte[,] templ157 = new byte[15, 15] { 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 0, 0, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 0, 0, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 0, 0, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 0, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 0, 0, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 0, 0, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 0, 0, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            };//template 157
        public byte[,] coreK = new byte[15, 15] { 
            {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
            {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
            {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
            {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
            {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
            {  0, 0, 0, 0, 0, 255, 255, 255, 255, 255, 0, 0, 0, 0, 0 }, 
            {  0, 0, 0, 0, 0, 255, 255, 255, 255, 255, 0, 0, 0, 0, 0 }, 
            {  0, 0, 0, 0, 0, 255, 255, 255, 255, 255, 0, 0, 0, 0, 0 }, 
            {  0, 0, 0, 0, 0, 255, 255, 255, 255, 255, 0, 0, 0, 0, 0 }, 
            {  0, 0, 0, 0, 0, 255, 255, 255, 255, 255, 0, 0, 0, 0, 0 }, 
            {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
            {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
            {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
            {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
            {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
            };//template marker core
        public byte[,] deltaK = new byte[15, 15] { 
            {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
            {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
            {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
            {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
            {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
            {  0, 0, 0, 0, 0, 0, 0, 255, 0, 0, 0, 0, 0, 0, 0 }, 
            {  0, 0, 0, 0, 0, 0, 255, 255, 255, 0, 0, 0, 0, 0, 0 }, 
            {  0, 0, 0, 0, 0, 0, 255, 255, 255, 0, 0, 0, 0, 0, 0 }, 
            {  0, 0, 0, 0, 0, 255, 255, 255, 255, 255, 0, 0, 0, 0, 0 }, 
            {  0, 0, 0, 0, 0, 255, 255, 255, 255, 255, 0, 0, 0, 0, 0 }, 
            {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
            {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
            {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
            {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
            {  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
            };//template marker delta
        public byte[,] bground = new byte[15, 15] { 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            {  255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, 
            };//template background
        #endregion

        string polaFP;
        string direct;
        string[] filelist;
        int urut = 0;

        public string ProsesKlas(Bitmap gambar)
        {
            List<int[]> corelist = new List<int[]>();
            List<int[]> deltalist = new List<int[]>();

            Fingerprint fp = new Fingerprint();
            fp.AsBitmap = gambar;
            fp.Finger = Finger.RightThumb;

            byte[,] image = ImageInverter.GetInverted(fp.Image);
            byte[,] hasil = ImageInverter.GetInverted(fp.Image);

            BlockMap blocks = new BlockMap(new SourceAFIS.General.Size(gambar.Width, gambar.Height), BlockSize);
            Logger.Log("BlockMap", blocks);

            short[, ,] histogram = Histogram.Analyze(blocks, image);
            short[, ,] smoothHistogram = Histogram.SmoothAroundCorners(blocks, histogram);
            BinaryMap mask = Mask.ComputeMask(blocks, histogram);               // contrast processed image
            float[,] equalized = Equalizer.Equalize(blocks, image, smoothHistogram, mask);

            byte[,] orientation = Orientation.Detect(equalized, mask, blocks);
            float[,] smoothed = RidgeSmoother.Smooth(equalized, orientation, mask, blocks);
            float[,] orthogonal = OrthogonalSmoother.Smooth(smoothed, orientation, mask, blocks);

            BinaryMap binary = Binarizer.Binarize(smoothed, orthogonal, mask, blocks);
            binary.AndNot(BinarySmoother.Filter(binary.GetInverted()));
            binary.Or(BinarySmoother.Filter(binary));
            Logger.Log("BinarySmoothingResult", binary);
            CrossRemover.Remove(binary);

            BinaryMap pixelMask = mask.FillBlocks(blocks);
            BinaryMap innerMask = InnerMask.Compute(pixelMask);     // Display segmented image mask

            // display orientasion map as byte                
            byte[,] oRi = new byte[orientation.GetLength(0) * BlockSize, orientation.GetLength(1) * BlockSize];

            // display orientation map as line
            byte[,][,] oRi2 = new byte[orientation.GetLength(0) * BlockSize, orientation.GetLength(1) * BlockSize][,];
            int[,] signGrad = new int[orientation.GetLength(0), orientation.GetLength(1)];
            for (int i = 0; i < orientation.GetLength(0); i++)
            {
                for (int j = 0; j < orientation.GetLength(1); j++)
                {
                    signGrad[i, j] = 5000; //signGrad[,][0] = grad +/- , signGrad[,][1] = vert/hor
                    if (innerMask.GetBit(j * 15, i * 15))
                    {
                        oRi2[i, j] = new byte[15, 15];
                        if (orientation[i, j] < 16 || orientation[i, j] >= 240)
                        {
                            oRi2[i, j] = templ0;
                            signGrad[i, j] = 0;
                        }
                        else if (orientation[i, j] < 48 && orientation[i, j] >= 16)
                        {
                            oRi2[i, j] = templ157;
                            signGrad[i, j] = 5;
                        }
                        else if (orientation[i, j] < 80 && orientation[i, j] >= 48)
                        {
                            oRi2[i, j] = templ135;
                            signGrad[i, j] = 5;
                        }
                        else if (orientation[i, j] < 112 && orientation[i, j] >= 80)
                        {
                            oRi2[i, j] = templ112;
                            signGrad[i, j] = 5;
                        }
                        else if (orientation[i, j] < 144 && orientation[i, j] >= 112)
                        {
                            oRi2[i, j] = templ90;
                            signGrad[i, j] = 1000;
                        }
                        else if (orientation[i, j] < 176 && orientation[i, j] >= 144)
                        {
                            oRi2[i, j] = templ67;
                            signGrad[i, j] = -5;
                        }
                        else if (orientation[i, j] < 208 && orientation[i, j] >= 176)
                        {
                            oRi2[i, j] = templ45;
                            signGrad[i, j] = -5;
                        }
                        else //if(orientation[i, j] < 240 && orientation[i, j] >= 208)
                        {
                            oRi2[i, j] = templ22;
                            signGrad[i, j] = -5;
                        }
                    }
                    else
                        oRi2[i, j] = bground;
                }
            }

            // display Orientation map
            for (int i = 0; i < orientation.GetLength(0); i++)
            {
                for (int j = 0; j < orientation.GetLength(1); j++)
                {
                    for (int x = 0; x < 15; x++)
                    {
                        for (int y = 0; y < 15; y++)
                        {
                            oRi[(i * 15) + x, (j * 15) + y] = oRi2[i, j][x, y];
                        }
                    }
                }
            }
            fp.Image = oRi;

            for (int i = 0; i < orientation.GetLength(0); i++)
            {
                for (int j = 0; j < orientation.GetLength(1); j++)
                {
                    if (innerMask.GetBit(j * 15, i * 15))
                    {
                        if (i != 0 && i != orientation.GetLength(0) - 1 && j != 0 && j != orientation.GetLength(1) - 1)
                        {
                            if (signGrad[i, j + 1] * signGrad[i, j - 1] < 0 && signGrad[i, j + 1] * signGrad[i, j - 1] > -500)
                            {
                                if (signGrad[i, j - 1] < 0 && orientation[i + 1, j] > 80 && orientation[i + 1, j] < 175 && (orientation[i - 1, j] > 48 || orientation[i - 1, j] > 208))
                                {
                                    oRi2[i, j] = coreK;
                                    int[] posisi = new int[3];
                                    posisi[0] = i;
                                    posisi[1] = j;
                                    corelist.Add(posisi);
                                }
                                else if (signGrad[i, j - 1] > 0 && orientation[i - 1, j] > 64 && orientation[i - 1, j] < 192 && (orientation[i + 1, j] < 48 || orientation[i + 1, j] > 208))
                                {
                                    oRi2[i, j] = coreK;
                                    int[] posisi = new int[3];
                                    posisi[0] = i;
                                    posisi[1] = j;
                                    corelist.Add(posisi);
                                }
                                else if (signGrad[i, j - 1] > 0 && orientation[i + 1, j] > 80 && orientation[i + 1, j] < 175 && (orientation[i - 1, j] < 48 || orientation[i - 1, j] > 208))
                                {
                                    oRi2[i, j] = deltaK;
                                    int[] posisi = new int[3];
                                    posisi[0] = i;
                                    posisi[1] = j;
                                    deltalist.Add(posisi);
                                }
                            }
                            else if (orientation[i, j + 1] < 48 || orientation[i, j + 1] > 208)
                            {
                                if (orientation[i - 1, j] > 80 && orientation[i - 1, j] < 175 && signGrad[i - 1, j + 1] < 0)
                                {
                                    oRi2[i, j] = coreK;
                                    int[] posisi = new int[3];
                                    posisi[0] = i;
                                    posisi[1] = j;
                                    corelist.Add(posisi);
                                }
                                else if (orientation[i + 1, j] > 80 && orientation[i + 1, j] < 175)
                                {
                                    if (signGrad[i + 1, j + 1] > 0 && signGrad[i - 1, j - 1] < 0)
                                    {
                                        oRi2[i, j] = coreK;
                                        int[] posisi = new int[3];
                                        posisi[0] = i;
                                        posisi[1] = j;
                                        corelist.Add(posisi);
                                    }
                                    else if (signGrad[i + 1, j + 1] < 0 && signGrad[i - 1, j - 1] > 0)
                                    {
                                        oRi2[i, j] = deltaK;
                                        int[] posisi = new int[3];
                                        posisi[0] = i;
                                        posisi[1] = j;
                                        deltalist.Add(posisi);
                                    }
                                }
                            }
                            else if (orientation[i, j - 1] < 48 || orientation[i, j - 1] > 208)
                            {
                                if (orientation[i - 1, j] > 80 && orientation[i - 1, j] < 175 && signGrad[i - 1, j - 1] > 0)
                                {
                                    oRi2[i, j] = coreK;
                                    int[] posisi = new int[3];
                                    posisi[0] = i;
                                    posisi[1] = j;
                                    corelist.Add(posisi);
                                }
                                else if (orientation[i + 1, j] > 80 && orientation[i + 1, j] < 175)
                                {
                                    if (signGrad[i + 1, j - 1] < 0 && signGrad[i - 1, j + 1] > 0)
                                    {
                                        oRi2[i, j] = coreK;
                                        int[] posisi = new int[3];
                                        posisi[0] = i;
                                        posisi[1] = j;
                                        corelist.Add(posisi);
                                    }
                                    else if (signGrad[i + 1, j - 1] > 0 && signGrad[i - 1, j + 1] < 0)
                                    {
                                        oRi2[i, j] = deltaK;
                                        int[] posisi = new int[3];
                                        posisi[0] = i;
                                        posisi[1] = j;
                                        deltalist.Add(posisi);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < orientation.GetLength(0); i++)
            {
                for (int j = 0; j < orientation.GetLength(1); j++)
                {
                    for (int x = 0; x < 15; x++)
                    {
                        for (int y = 0; y < 15; y++)
                        {
                            oRi[(i * 15) + x, (j * 15) + y] = oRi2[i, j][x, y];
                        }
                    }
                }
            }

            fp.Image = oRi;

            List<int[]> validcore = new List<int[]>();
            List<int[]> validdelta = new List<int[]>();

            int jumlah = 0;
            for (int i = 0; i < deltalist.Count(); i++)
            {
                bool found = false;
                if (i != 0)
                {
                    for (int j = 0; j < i; j++)
                    {
                        if ((deltalist.ElementAt(i)[0] - deltalist.ElementAt(j)[0] <= 1 && deltalist.ElementAt(i)[0] - deltalist.ElementAt(j)[0] >= -1) && (deltalist.ElementAt(i)[1] - deltalist.ElementAt(j)[1] <= 1 && deltalist.ElementAt(i)[1] - deltalist.ElementAt(j)[1] >= -1))
                        {
                            deltalist.ElementAt(i)[2] = deltalist.ElementAt(j)[2];
                            found = true;
                        }
                    }
                }
                if (!found)
                {
                    jumlah++;
                    deltalist.ElementAt(i)[2] = jumlah;
                    validdelta.Add(deltalist.ElementAt(i));
                }
            }

            jumlah = 0;
            for (int i = 0; i < corelist.Count(); i++)
            {
                bool found = false;
                if (i != 0)
                {
                    for (int j = 0; j < i; j++)
                    {
                        if ((corelist.ElementAt(i)[0] - corelist.ElementAt(j)[0] <= 1 && corelist.ElementAt(i)[0] - corelist.ElementAt(j)[0] >= -1) && (corelist.ElementAt(i)[1] - corelist.ElementAt(j)[1] <= 1 && corelist.ElementAt(i)[1] - corelist.ElementAt(j)[1] >= -1))
                        {
                            corelist.ElementAt(i)[2] = corelist.ElementAt(j)[2];
                            found = true;
                        }
                    }
                }
                if (!found)
                {
                    if (validdelta.Count() != 0)
                    {
                        if (corelist.ElementAt(i)[0] - validdelta.ElementAt(0)[0] >= 1)
                        {
                            jumlah++;
                            corelist.ElementAt(i)[2] = jumlah;
                            validcore.Add(corelist.ElementAt(i));
                        }
                    }
                    else
                    {
                        jumlah++;
                        corelist.ElementAt(i)[2] = jumlah;
                        validcore.Add(corelist.ElementAt(i));
                    }
                }
            }

            if (validcore.Count() == 0)
            {
                polaFP = "Arch";
            }
            else if (validcore.Count() == 2)
            {
                polaFP = "Whorl";
            }
            else if (validcore.Count() == 1)
            {
                if (validdelta.Count() > 0)
                {
                    if (validdelta.ElementAt(0)[1] < validcore.ElementAt(0)[1])
                    {
                        polaFP = "Right Loop";
                    }
                    else if (validdelta.ElementAt(0)[1] > validcore.ElementAt(0)[1])
                    {
                        polaFP = "Left Loop";
                    }
                    else
                    {
                        polaFP = "Tented Arch";
                    }
                }
                else
                    polaFP = "unKnown";
            }
            else
                polaFP = "unKnown";

            return polaFP;
        }
    }
}

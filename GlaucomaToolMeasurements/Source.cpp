#include "Header.h"

IplImage *cup , *disc, *rim, *I_mask, *S_mask, *N_mask, *T_mask, *gray ,* input_cup , *input_disc , *lineInterceptImg,*tempMask_float,*tempMask;						//Images created

double area_cup,area_disc, area_rim, area_I, area_S,area_N, area_T;						//Areas

CvPoint upLeft_cup, upRight_cup, downLeft_cup, downRight_cup, center_cup;				//Cup points

CvPoint upLeft_disc, upRight_disc, downLeft_disc, downRight_disc, center_disc;			//Disc points

CvPoint upLeft_intercept, upRight_intercept, downLeft_intercept, downRight_intercept;	//Intercept points

CvRect rect_cup, rect_disc;

double downLeft_shift, downRight_shift, upLeft_shift, upRight_shift, slope_1, slope_2;

double whiteVertical_disc, whiteHorizontal_disc, whiteVertical_cup, whiteHorizontal_cup;



extern "C" _declspec(dllexport) void CDRInit(int width,int height)
{
	cup = cvCreateImage (cvSize(width,height), IPL_DEPTH_8U,1);
	disc = cvCreateImage (cvSize(width,height), IPL_DEPTH_8U,1);
	rim = cvCreateImage (cvSize(width,height), IPL_DEPTH_8U,3);

	I_mask = cvCreateImage (cvGetSize(rim), rim->depth, rim->nChannels);
	S_mask = cvCreateImage (cvGetSize(rim), rim->depth, rim->nChannels);
	N_mask = cvCreateImage (cvGetSize(rim), rim->depth, rim->nChannels);
	T_mask = cvCreateImage (cvGetSize(rim), rim->depth, rim->nChannels);
	input_cup = cvCreateImage (cvGetSize(rim), rim->depth, 3);
	input_disc = cvCreateImage (cvGetSize(rim), rim->depth, 3);
	gray = cvCreateImage (cvGetSize(rim), rim->depth, cup->nChannels);
	lineInterceptImg = cvCreateImage (cvGetSize(rim), rim->depth, 3);
	tempMask = cvCreateImage (cvGetSize(rim), rim->depth, 1);
	tempMask_float = cvCreateImage (cvGetSize(rim), IPL_DEPTH_32F, 1);
}
extern "C" _declspec(dllexport) void CDRExit()
{
	cvReleaseImage(&cup);
	cvReleaseImage(&disc);
	cvReleaseImage(&rim);
	cvReleaseImage(&I_mask);
	cvReleaseImage(&S_mask);
	cvReleaseImage(&N_mask);
	cvReleaseImage(&T_mask);
	cvReleaseImage(&tempMask_float);
	cvReleaseImage(&tempMask);
	cvReleaseImage(&gray);
	cvReleaseImage(&lineInterceptImg);


}

extern "C" __declspec(dllexport) Cup2DiscStruct* CalculateCDR( char* discImagePtr ,  char* cupImagePtr)
{
	//input images ********************************************************************************************************************************************************************** 
	/*IplImage * input_cup = cvLoadImage("E:\\Suhasini\\cup.bmp", CV_LOAD_IMAGE_UNCHANGED);
	IplImage *input_disc = cvLoadImage("E:\\Suhasini\\disk.bmp", CV_LOAD_IMAGE_UNCHANGED);*/ // used for exe only 
	
	// create images required*******************************************************************************************************************************************************************
	
	Cup2DiscStruct *_cup2DiscValues = new  Cup2DiscStruct();
	input_cup->imageData = cupImagePtr;
	input_disc->imageData = discImagePtr;
	// Get Rim*******************************************************************************************************************************************************************
	cvSub(input_disc,input_cup,rim );
		/*cvNamedWindow("rim",CV_WINDOW_AUTOSIZE);
		cvShowImage("rim",rim );*/
	
	//Bounding rectangle and area for cup, disc and rim******************************************************************************************************************************************

	GetBoundingRectangle (input_cup,rim,&rect_cup);

	//cvNamedWindow("check",CV_WINDOW_AUTOSIZE);
	//cvShowImage("check",rim );
	area_cup =ComputeArea(input_cup);
	_cup2DiscValues->CupArea = area_cup;
	//cout<< "Area_cup =  "<<area_cup<<"\n"<<endl;

	//Disc rectangle and area********************************************************************************************************
	
	GetBoundingRectangle (input_disc,rim,&rect_disc);
	//cvNamedWindow("check",CV_WINDOW_AUTOSIZE);
	//cvShowImage("check",rim );
	area_disc =ComputeArea(input_disc);
	_cup2DiscValues->DiscArea = area_disc;
	//cout<< "Area_disc =  "<<area_disc<<"\n"<<endl;
	
	//Rim area********************************************************************************************************
	
	area_rim =area_disc-area_cup;
	//cout<< "Area_rim =  "<<area_rim<<"\n"<<endl;
	_cup2DiscValues->RimArea = area_rim;
		
	// Get points and there co-ordinates*************************************************************************************************************************************************

	GetPoints(rect_cup,rect_disc);
	
	//Diagonals of cup rectangle
	//cvLine(rim,upLeft_cup,downRight_cup,cvScalarAll(100),1,8,0);
	//cvLine(rim,downLeft_cup,upRight_cup,cvScalarAll(100),1,8,0);
	// extending diagonals to disc boundry******************************************************************************************************************************************

	computeLineProjection();
	

 	//cvDrawRect(rim,cvPoint(rect_disc.x,rect_disc.y),cvPoint(rect_disc.x+rect_disc.width,rect_disc.y+rect_disc.height),cvScalarAll(100));

	
	//cvNamedWindow("Result with ISNT regions",CV_WINDOW_AUTOSIZE);
	//cvShowImage("Result with ISNT regions",rim );

// Create I_mask and get its area*********************************************************************************************
	computeMask(center_cup,upLeft_intercept,downLeft_intercept,rim,T_mask,3);

	computeMask(center_cup,upLeft_intercept,upRight_intercept,rim,S_mask,1);

	computeMask(center_cup,upRight_intercept,downRight_intercept,rim,N_mask,4);
	
	computeMask(center_cup,downRight_intercept,downLeft_intercept,rim,I_mask,2);

	//GetInferiormask(rim, I_mask);

	area_I =ComputeArea(I_mask);
	_cup2DiscValues->InferiorRegionArea = area_I;
	

	// Create S_mask and get its area*********************************************************************************************

	//GetSuperiormask(rim, S_mask);
	area_S =ComputeArea(S_mask);
	/*cvSaveImage("S_mask.png",S_mask);
	cvSaveImage("T_mask.png",T_mask);
	cvSaveImage("I_mask.png",I_mask);
    cvSaveImage("N_mask.png",N_mask);*/

	_cup2DiscValues->SuperiorRegionArea = area_S;
	
	//	// Create N_mask and get its area*********************************************************************************************

	//GetNasalmask(rim, N_mask);

	area_N =ComputeArea(N_mask);
	_cup2DiscValues->NasalRegionArea = area_N;
	
//		// Create T_mask and get its area*********************************************************************************************
	
	//GetTemporalmask(rim, T_mask);
	//cvSaveImage("T_mask.png",T_mask);

	area_T =ComputeArea(T_mask);
	_cup2DiscValues->TemporalRegionArea = area_T;
	
	//*cvNamedWindow("maskImage_I",CV_WINDOW_AUTOSIZE);
	//cvShowImage("maskImage_I",I_mask );*/
	//*cvNamedWindow("maskImage_S",CV_WINDOW_AUTOSIZE);
	//cvShowImage("maskImage_S",S_mask );*/
	//*cvNamedWindow("maskImage_N",CV_WINDOW_AUTOSIZE);
	//cvShowImage("maskImage_N",N_mask );*/
	//*cvNamedWindow("maskImage_T",CV_WINDOW_AUTOSIZE);
	//cvShowImage("maskImage_T",T_mask );*/

	/*cout<< "Area_I =  "<<area_I<<"\n"<<endl;
	cout<< "Area_S =  "<<area_S<<"\n"<<endl;
	cout<< "Area_N =  "<<area_N<<"\n"<<endl;
	cout<< "Area_T =  "<<area_T<<"\n"<<endl;*/


//*****Comput number of horizontal and vertical white pixels and CDR*****************************************************************************************
	
	GetWhitePixels(input_disc, input_cup);
	_cup2DiscValues->VerticalLengthDisc = whiteVertical_disc;
	_cup2DiscValues->VerticalLengthCup = whiteVertical_cup;

	_cup2DiscValues->HorizontalLengthDisc = whiteHorizontal_disc;
	_cup2DiscValues->HorizontalLengthCup = whiteHorizontal_cup;
	/*cout<< "whiteVertical_disc =  "<<whiteVertical_disc<<"\n"<<endl;
	cout<< "whiteHorizontal_disc =  "<<whiteHorizontal_disc<<"\n"<<endl;	
	cout<< "whiteVertical_cup =  "<<whiteVertical_cup<<"\n"<<endl;
	cout<< "whiteHorizontal_cup =  "<<whiteHorizontal_cup<<"\n"<<endl;*/

	double CDR_vertical= ComputeCDR(whiteVertical_cup,whiteVertical_disc);
	double CDR_horizontal=ComputeCDR(whiteHorizontal_cup,whiteHorizontal_disc);
	_cup2DiscValues->VerticalCDR = CDR_vertical;
	_cup2DiscValues->HorizontalCDR= CDR_horizontal;
	//cout<< "CDR_vertical =  "<<CDR_vertical<<"\n"<<endl;
	//cout<< "CDR_horizontal =  "<<CDR_horizontal<<"\n"<<endl;
	return _cup2DiscValues;
}
//int main( int argc, char** argv )
//{
//	//input images ********************************************************************************************************************************************************************** 
//	IplImage * input_cup = cvLoadImage("E:\\Suhasini\\cup.bmp", CV_LOAD_IMAGE_UNCHANGED);
//	IplImage *input_disc = cvLoadImage("E:\\Suhasini\\disk.bmp", CV_LOAD_IMAGE_UNCHANGED);
//	
//	// create images required*******************************************************************************************************************************************************************
//	
//	
//	// Get Rim*******************************************************************************************************************************************************************
//	cvSub(input_disc,input_cup,rim );
//		cvNamedWindow("rim",CV_WINDOW_AUTOSIZE);
//		cvShowImage("rim",rim );
//
//	//Bounding rectangle and area for cup, disc and rim******************************************************************************************************************************************
//
//	GetBoundingRectangle (input_cup,rim,&rect_cup);
//
//	//cvNamedWindow("check",CV_WINDOW_AUTOSIZE);
//	//cvShowImage("check",rim );
//	
//	area_cup =ComputeArea(input_cup);
//	cout<< "Area_cup =  "<<area_cup<<"\n"<<endl;
//
//	//Disc rectangle and area********************************************************************************************************
//	
//	GetBoundingRectangle (input_disc,rim,&rect_disc);
//	//cvNamedWindow("check",CV_WINDOW_AUTOSIZE);
//	//cvShowImage("check",rim );
//	
//	area_disc =ComputeArea(input_disc);
//	cout<< "Area_disc =  "<<area_disc<<"\n"<<endl;
//	
//	//Rim area********************************************************************************************************
//	
//	area_rim =area_disc-area_cup;
//	cout<< "Area_rim =  "<<area_rim<<"\n"<<endl;
//		
//	// Get points and there co-ordinates*************************************************************************************************************************************************
//
//	GetPoints(rect_cup,rect_disc);
//	
//	//Diagonals of cup rectangle
//	//cvLine(rim,upLeft_cup,downRight_cup,cvScalarAll(100),1,8,0);
//	//cvLine(rim,downLeft_cup,upRight_cup,cvScalarAll(100),1,8,0);
//
//	// extending diagonals to disc boundry******************************************************************************************************************************************
//
//	computeLineProjection();
//	
//	cvNamedWindow("Result with ISNT regions",CV_WINDOW_AUTOSIZE);
//	cvShowImage("Result with ISNT regions",rim );
//
//// Create I_mask and get its area*********************************************************************************************
//
//	GetInferiormask(rim, I_mask);
//	area_I =ComputeArea(I_mask);
//	
//
//	// Create S_mask and get its area*********************************************************************************************
//
//	GetSuperiormask(rim, S_mask);
//	area_S =ComputeArea(S_mask);
//	
//	//	// Create N_mask and get its area*********************************************************************************************
//
//	GetNasalmask(rim, N_mask);
//	area_N =ComputeArea(N_mask);
//	
////		// Create T_mask and get its area*********************************************************************************************
//	
//	GetTemporalmask(rim, T_mask);
//	area_T =ComputeArea(T_mask);
//	
//	//*cvNamedWindow("maskImage_I",CV_WINDOW_AUTOSIZE);
//	//cvShowImage("maskImage_I",I_mask );*/
//	//*cvNamedWindow("maskImage_S",CV_WINDOW_AUTOSIZE);
//	//cvShowImage("maskImage_S",S_mask );*/
//	//*cvNamedWindow("maskImage_N",CV_WINDOW_AUTOSIZE);
//	//cvShowImage("maskImage_N",N_mask );*/
//	//*cvNamedWindow("maskImage_T",CV_WINDOW_AUTOSIZE);
//	//cvShowImage("maskImage_T",T_mask );*/
//
//	cout<< "Area_I =  "<<area_I<<"\n"<<endl;
//	cout<< "Area_S =  "<<area_S<<"\n"<<endl;
//	cout<< "Area_N =  "<<area_N<<"\n"<<endl;
//	cout<< "Area_T =  "<<area_T<<"\n"<<endl;
//
//
////*****Comput number of horizontal and vertical white pixels and CDR*****************************************************************************************
//	
//	GetWhitePixels(input_disc, input_cup);
//
//	cout<< "whiteVertical_disc =  "<<whiteVertical_disc<<"\n"<<endl;
//	cout<< "whiteHorizontal_disc =  "<<whiteHorizontal_disc<<"\n"<<endl;	
//	cout<< "whiteVertical_cup =  "<<whiteVertical_cup<<"\n"<<endl;
//	cout<< "whiteHorizontal_cup =  "<<whiteHorizontal_cup<<"\n"<<endl;
//
//	double CDR_vertical= ComputeCDR(whiteVertical_cup,whiteVertical_disc);
//	double CDR_horizontal=ComputeCDR(whiteHorizontal_cup,whiteHorizontal_disc);
//	cout<< "CDR_vertical =  "<<CDR_vertical<<"\n"<<endl;
//	cout<< "CDR_horizontal =  "<<CDR_horizontal<<"\n"<<endl;
//
//	cvWaitKey(-1);
//}

double ComputeCDR (double x, double y)
{
  return x / y;
}

double ComputeArea (IplImage* img)
{
	double area ;	
	cvSplit(img,gray,0,0,0);
	
	CvMemStorage *storage = cvCreateMemStorage(0);
	CvSeq *contours = cvCreateSeq(0, sizeof(CvSeq), sizeof(CvPoint), storage);
	int contourCnt = cvFindContours(gray, storage, &contours, sizeof(CvContour), CV_RETR_LIST,
		CV_CHAIN_APPROX_SIMPLE, cvPoint(0,0));
	for ( ; contours!=0; contours = contours->h_next)
	{
		cvDrawContours(gray,contours,cvScalarAll(255),cvScalarAll(100),255,1);
		area =	cvContourArea(contours);
	}
	
	return area;
}

void GetBoundingRectangle(IplImage * srcImg, IplImage* dstImg, CvRect *BoundingRect)
{
	cvCvtColor(srcImg,gray,CV_BGR2GRAY);

	CvMemStorage *storage = cvCreateMemStorage(0);
	CvSeq *contours = cvCreateSeq(0, sizeof(CvSeq), sizeof(CvPoint), storage);
	cvFindContours(gray, storage, &contours, sizeof(CvContour), CV_RETR_LIST,
		CV_CHAIN_APPROX_SIMPLE, cvPoint(0,0));
	CvRect localRect = cvBoundingRect(contours);
	 *BoundingRect =	cvBoundingRect(contours);

	for ( ; contours!=0; contours = contours->h_next)
	{
		cvDrawContours(dstImg,contours,cvScalarAll(255),cvScalarAll(100),255,1);
		//cvDrawRect(dstImg,cvPoint(localRect .x,localRect .y),cvPoint(localRect .x+localRect .width,localRect .y +localRect .height),cvScalarAll(100),1);
	}
}

void GetInferiormask(IplImage * srcImg, IplImage* dstImg)
{
	
	bool isStart_i = false;
	int startPoint = 0;
	int endPoint = 0;
	if(downLeft_intercept.x == rect_disc.x )
		startPoint = downLeft_intercept.x;
	else
		startPoint = downLeft_intercept.x;
	if(downRight_intercept.x == rect_disc.x + rect_disc.width)
		endPoint = rect_disc.x + rect_disc.width;
	else
		endPoint = downRight_intercept.x; 
	for (int i = startPoint; i <= endPoint; i++)
	{
		isStart_i = false;
		for (int j = rect_disc.y+rect_disc.height; j >= center_cup.y; j--)
		{
		//	if(!isStart_i)
		//	{
		//		double m_start = ((double)i-downLeft_shift)/double(j);
		//		if (m_start==slope_2)
		//			{
		//				isStart_i = true;
		//			}
		//	}
		//	else
		//	{
		//		double m_stop =((double)i-downRight_shift)/double(j);
		//		if(m_stop == slope_1)
		//			isStart_i = false;
		//	     if (m_stop > slope_1)
		//		{
		//			CvScalar resultPixVal = cvGet2D(srcImg,i,j);
		//			cvSet2D(dstImg,i,j,resultPixVal);
 	//				//break;
		//		}
		//	
		//}
			CvScalar retPixelVal = cvGet2D(srcImg,j,i);
			if(i > center_cup.x)
			{
				if(retPixelVal.val[1] != 255)
					cvSet2D(dstImg,j,i,retPixelVal);
				else
				break;
			}
			else if(i >= center_cup.x)
			{
				if(retPixelVal.val[0] != 255)
					cvSet2D(dstImg,j,i,retPixelVal);
				else
				break;
			}
	    }
	}
}
	
void GetSuperiormask(IplImage * srcImg, IplImage* dstImg)
{
	bool isStart_s = false;
	int startPoint = 0;
	if(upLeft_intercept.y == rect_disc.y)
		startPoint = upLeft_intercept.y;
	else
		startPoint =upLeft_intercept.y;
	for (int i = startPoint; i <= center_cup.y; i++)
	{
		isStart_s = false;
		for (int j = upLeft_intercept.x; j <= upRight_intercept.x; j++)
		{
			//if(!isStart_s)
			//{
			//	double m_start = ((double)i-upLeft_shift)/double(j);

			//	if (m_start==slope_1)
			//	{
			//		isStart_s = true;
			//	}
			//}
			//else
			//{
			//	double m_stop =((double)i-upRight_shift)/double(j);
			//	if(m_stop == slope_2)
			//		isStart_s = false;
			//     if (m_stop < slope_2)
			//	{
			//		CvScalar resultPixVal = cvGet2D(srcImg,i,j);
			//		cvSet2D(dstImg,i,j,resultPixVal);
 		//			//break;
			//	}
			//}
			CvScalar retPixelVal = cvGet2D(srcImg,i,j);
			if(i < center_cup.y)
			{
				if(retPixelVal.val[0] != 255)
					cvSet2D(dstImg,i,j,retPixelVal);
				else
					break;
			}
			else if(i >= center_cup.y)
			{
				if(retPixelVal.val[1] != 255)
					cvSet2D(dstImg,i,j,retPixelVal);
				else
					break;
			}
		}
	}
	
}

void GetNasalmask(IplImage * srcImg, IplImage* dstImg)
{
	bool isStart_N = false;
	int startPoint = 0;
	int endPoint = 0;
	if(upRight_intercept.y == rect_disc.y)
		startPoint = upRight_intercept.y;
	else
		startPoint = upRight_intercept.y;
	if(downRight_intercept.y == rect_disc.y+rect_disc.height)
		endPoint = rect_disc.y+rect_disc.height;
	else
		endPoint = downRight_intercept.y;
	for (int i =startPoint; i<= endPoint; i++)
	{
		isStart_N= false;
		for (int j = rect_disc.x + rect_disc.width; j  >= center_cup.x; j--)
		{
			CvScalar retPixelVal = cvGet2D(srcImg,i,j);
			if(i < center_cup.y)
			{
				if(retPixelVal.val[1] != 255)
					cvSet2D(dstImg,i,j,retPixelVal);
				else
					break;
			}
			else if(i>= center_cup.y)
			{
				if(retPixelVal.val[0] != 255)
					cvSet2D(dstImg,i,j,retPixelVal);
				else
					break;
			}
			/*if(!isStart_N)
			{
				double m_start1 = ((double)i-upRight_shift)/double(j);
				double m_start2 = ((double)i-downRight_shift)/double(j);

				if (m_start1 > slope_2 && m_start2<slope_1)
				{
					isStart_N = true;
				}
			}

			else
			{	
				CvScalar resultPixVal_N = cvGet2D(srcImg,i,j);
				cvSet2D(dstImg,i,j,resultPixVal_N);
			}*/
		}
	}
}

void GetTemporalmask(IplImage * srcImg, IplImage* dstImg)
{
bool isStart_T = false;
int startPoint = 0;
int endPoint = 0;
if(upLeft_intercept.y == rect_disc.y)
	startPoint = rect_disc.y;
else
	startPoint = upLeft_intercept.y;
if(downLeft_intercept.y == rect_disc.y + rect_disc.height)
	endPoint = rect_disc.y + rect_disc.height;
else
	endPoint = downLeft_intercept.y; 
for (int i = startPoint; i<= endPoint; i++)
	{
	isStart_T= false;
		for (int j = upLeft_disc.x; j  <= center_cup.x; j++)
		{
			CvScalar retPixelVal = cvGet2D(srcImg,i,j);

			if(i < center_cup.y )
			{
				if(retPixelVal.val[0] != 255)
					cvSet2D(dstImg,i,j,retPixelVal);
				else
					break;

			}
			else if(i >= center_cup.y)
			{
				if(retPixelVal.val[1] != 255)
					cvSet2D(dstImg,i,j,retPixelVal);
				else
					break;
			}

			/*if(!isStart_T)
			{
				
				if (i <= downLeft_intercept.y && j <= center_cup.x)
				{
					isStart_T = true;
				}
			}

			else
			{
				double m_stop1 = ((double)i-upLeft_shift)/double(j);
				double m_stop2 = ((double)i-downLeft_shift)/double(j);

				if (m_stop1 > slope_1 && m_stop2<slope_2)
				{
					CvScalar resultPixVal_T = cvGet2D(srcImg,i,j);
					cvSet2D(dstImg,i,j,resultPixVal_T);
				}
			}*/
		}
	}
}

void GetPoints(CvRect cupRect, CvRect discRect )
{
	upLeft_cup=cvPoint (cupRect.x , cupRect.y);													
	upRight_cup=cvPoint (cupRect.x + cupRect.width , cupRect.y);								
	downLeft_cup=cvPoint (cupRect.x , cupRect.y + cupRect.height);							
	downRight_cup=cvPoint (cupRect.x + cupRect.width , cupRect.y + cupRect.height);				
	center_cup=cvPoint ((cupRect.x + cupRect.width/2) , (cupRect.y + cupRect.height/2));	

	upLeft_disc=cvPoint (discRect.x , discRect.y);											
	upRight_disc=cvPoint (discRect.x + discRect.width , discRect.y);									
	downLeft_disc=cvPoint (discRect.x , discRect.y + discRect.height);						
	downRight_disc=cvPoint (discRect.x + discRect.width , discRect.y + discRect.height);			
	center_disc=cvPoint ((discRect.x + discRect.width/2) , (discRect.y + discRect.height/2));			
	
}

void GetWhitePixels(IplImage * discImage, IplImage *cupImage)
{
	cvCvtColor(discImage,disc,CV_BGR2GRAY);
		IplImage * rectImage_disc = cvCreateImage(cvGetSize(disc),disc->depth,disc->nChannels);
		cvCopy(disc,rectImage_disc);

		CvRect verticalRect_disc = cvRect(center_cup.x,0,1,disc->height);

		cvSetImageROI(rectImage_disc,verticalRect_disc);
				
	whiteVertical_disc=cvCountNonZero(rectImage_disc);

	cvResetImageROI(rectImage_disc);
	
		CvRect horizontalRect_disc = cvRect(0,center_cup.y,disc->width,1);

		cvSetImageROI(rectImage_disc,horizontalRect_disc);
				
	whiteHorizontal_disc=cvCountNonZero(rectImage_disc);
//****************************************************************************************************************************************
	
	cvCvtColor(cupImage,cup,CV_BGR2GRAY);
		IplImage * rectImage_cup = cvCreateImage(cvGetSize(cup),cup->depth,cup->nChannels);
		cvCopy(cup,rectImage_cup);
		CvRect verticalRect_cup = cvRect(center_cup.x,0,1,cup->height);
		cvSetImageROI(rectImage_cup,verticalRect_cup);
	whiteVertical_cup=cvCountNonZero(rectImage_cup); 
	cvResetImageROI(rectImage_cup);
	
		CvRect horizontalRect_cup = cvRect(0,center_cup.y,disc->width,1);

		cvSetImageROI(rectImage_cup,horizontalRect_cup);
				
	whiteHorizontal_cup=cvCountNonZero(rectImage_cup); 
}

void computeLineProjection()
{
	slope_1=(double)(downRight_cup.y-upLeft_cup.y)/(double)(downRight_cup.x-upLeft_cup.x);					// m = (y2-y1)/(x2-x1)
	//cvDrawRect(lineInterceptImg,cvPoint(rect_disc.x,rect_disc.y) , cvPoint(rect_disc.x + rect_disc.width,rect_disc.y + rect_disc.height),cvScalarAll(100),1,CV_AA);
	upLeft_shift = upLeft_cup.y - (slope_1*upLeft_cup.x);										// c = y-mx
	int upLeft_y = upLeft_disc.y;					
	int upLeft_x=((upLeft_y-upLeft_shift)/slope_1);
	upLeft_intercept = cvPoint(upLeft_x,upLeft_y);
	cvLine(lineInterceptImg,center_cup,upLeft_intercept,cvScalar(255,0,0,0),1,8,0);

	//cv::Point2f result;
	///*bool retVal = 	LineInterSection(downRight_cup, upLeft_cup,cvPoint(rect_disc.x,rect_disc.y),
	//	cvPoint(rect_disc.x+rect_disc.width,rect_disc.y),&upLeft_intercept);*/
	//bool retVal = 	intersection(cv::Point2f(downRight_cup.x,downRight_cup.y),cv::Point2f(upLeft_cup.x,upLeft_cup.y) ,cv::Point2f(rect_disc.x,rect_disc.y),
	//	cv::Point2f(rect_disc.x+rect_disc.width,rect_disc.y),result);
	//
	//if(!retVal)
	//		retVal = intersection(cv::Point2f(downRight_cup.x,downRight_cup.y),cv::Point2f(upLeft_cup.x,upLeft_cup.y) ,cv::Point2f(rect_disc.x,rect_disc.y),
	//			cv::Point2f(rect_disc.x,rect_disc.y + rect_disc.height),result);
	//cvLine(rim,center_cup,result,cvScalar(255,0,0,0),1,8,0);

	//		
	// retVal = 	intersection(cv::Point2f(downRight_cup.x,downRight_cup.y),cv::Point2f(upLeft_cup.x,upLeft_cup.y) ,cv::Point2f(rect_disc.x +rect_disc.width,rect_disc.y),
	//	cv::Point2f(rect_disc.x+rect_disc.width,rect_disc.y+ rect_disc.height),result);
	//if(!retVal)
	//	retVal = 	intersection(cv::Point2f(downRight_cup.x,downRight_cup.y),cv::Point2f(upLeft_cup.x,upLeft_cup.y) ,cv::Point2f(rect_disc.x ,rect_disc.y +rect_disc.height),
	//	cv::Point2f(rect_disc.x+rect_disc.width,rect_disc.y+ rect_disc.height),result);

	//cvDrawCircle(rim,upLeft_intercept,10,cvScalarAll(255),10,8,0);
	
	//upLeft_intercept = cvPoint(upLeft_x,upLeft_y);



	downRight_shift=(downRight_cup.y - (slope_1 * downRight_cup.x));
	int downRight_x=downRight_disc.x;
	int downRight_y=(slope_1*downRight_x)+downRight_shift;
	
	downRight_intercept = cvPoint(downRight_x,downRight_y);

	cvLine(lineInterceptImg,center_cup,downRight_intercept,cvScalar(255,0,0,0),1,8,0);
	
	////slope for diagonal two*********************************************************
	slope_2=(double)(downLeft_cup.y-upRight_cup.y)/(double)(downLeft_cup.x-upRight_cup.x);

	upRight_shift=(upRight_cup.y- (slope_2 * upRight_cup.x));
	int upRight_x=upRight_disc.x;
	int upRight_y=(slope_2*upRight_x)+upRight_shift;

	upRight_intercept = cvPoint(upRight_x,upRight_y);

	cvLine(lineInterceptImg,center_cup,upRight_intercept,cvScalar(0,255,0,0),1,8,0);

	downLeft_shift=(downLeft_cup.y - (slope_2 * downLeft_cup.x));
	int downLeft_y=downLeft_disc.y;
	int downLeft_x=((downLeft_y-downLeft_shift)/slope_2);

	downLeft_intercept = cvPoint(downLeft_x,downLeft_y);
	cvLine(lineInterceptImg,center_cup,downLeft_intercept,cvScalar(0,255,0,0),1,8,0);
	//cvSaveImage("line.png",lineInterceptImg);
	for (int i = rect_disc.y; i < rect_disc.y + rect_disc.height; i++)
	{
		CvScalar val =	cvGet2D(lineInterceptImg,i,rect_disc.x);
		if(val.val[0] == 255)
			upLeft_intercept = cvPoint(rect_disc.x,i);
		else if(val.val[1] == 255)
			downLeft_intercept = cvPoint(rect_disc.x,i);
	}
	for (int i = rect_disc.x; i < rect_disc.x + rect_disc.width; i++)
	{
		int x = i;
		
		CvScalar val =	cvGet2D(lineInterceptImg,rect_disc.y,i);
		if(val.val[0] == 255)
			downRight_intercept = cvPoint(i,rect_disc.y + rect_disc.height);
		else if(val.val[1] == 255)
			downLeft_intercept = cvPoint(i,rect_disc.y + rect_disc.height);
	}
	for (int i = rect_disc.y; i < rect_disc.y + rect_disc.height; i++)
	{
		CvScalar val =	cvGet2D(lineInterceptImg,i,rect_disc.x+rect_disc.width);
		if(val.val[0] == 255)
			downRight_intercept = cvPoint(rect_disc.x+rect_disc.width,i);
		else if(val.val[1] == 255)
			upRight_intercept = cvPoint(rect_disc.x+rect_disc.width,i);
	}
	for (int i = rect_disc.x; i < rect_disc.x + rect_disc.width; i++)
	{
		CvScalar val =	cvGet2D(lineInterceptImg,rect_disc.y,i);
		if(val.val[0] == 255)
			upLeft_intercept = cvPoint(i,rect_disc.y );
		else if(val.val[1] == 255)
			upRight_intercept = cvPoint(i,rect_disc.y);
	}
}

bool LineInterSection(CvPoint srcPt1, CvPoint srcpt2, CvPoint startPt1,CvPoint endpt1, CvPoint* endpt2)
{
	double val1 = srcpt2.y - srcPt1.y;
	double val2 = srcpt2.x - srcPt1.x;

	double A1 = srcpt2.y - srcPt1.y;
	double B1 = srcPt1.x - srcpt2.x;
	double C1 = A1* (double) srcPt1.x+ B1 * (double) srcPt1.y;

	double A2 = endpt1.y - startPt1.y;
	double B2 = startPt1.x - endpt1.x;
	double C2 = A2* (double) startPt1.x+ B2 * (double) startPt1.y;

	/*double SlopeM1 = (double) (val1)/(double) (val2);
	val1 = startPt1.y - endpt1.y;
	val2 = startPt1.x - endpt1.x;

	double SlopeM2 = (double) (val1)/(double) (val2);
	CvPoint intersectopnPnt;
	double c1 = (double)srcpt2.y - SlopeM1 * (double)srcpt2.x;
	double c2 = (double)startPt1.y - SlopeM2 * (double)startPt1.x;*/
	double det = A1 * B2 - A2 * B1;
	//if((SlopeM1 - SlopeM2)  == 0)
	if(det  == 0)
	{
		return false;
	}
	else
	{
		
		//double valX = (c2 - c1) / (SlopeM1 -SlopeM2);
		//intersectopnPnt.x = valX;

	 //  //intersectopnPnt.y = intersectopnPnt.x * SlopeM1 + c1;
		//double ValY = valX * SlopeM1 + c1;
		//intersectopnPnt.y = ValY;
	 //  *endpt2 = intersectopnPnt;
		double x = (B2*C1 - B1*C2)/det;
        double y = (A1*C2 - A2*C1)/det;
		*endpt2 = cvPoint(x,y);
	   return true;
	}
}

char get_line_intersection(float p0_x, float p0_y, float p1_x, float p1_y, 
    float p2_x, float p2_y, float p3_x, float p3_y, float *i_x, float *i_y)
{
    float s1_x, s1_y, s2_x, s2_y;
    s1_x = p1_x - p0_x;     s1_y = p1_y - p0_y;
    s2_x = p3_x - p2_x;     s2_y = p3_y - p2_y;

    float s, t;
    s = (-s1_y * (p0_x - p2_x) + s1_x * (p0_y - p2_y)) / (-s2_x * s1_y + s1_x * s2_y);
    t = ( s2_x * (p0_y - p2_y) - s2_y * (p0_x - p2_x)) / (-s2_x * s1_y + s1_x * s2_y);

    if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
    {
        // Collision detected
        if (i_x != NULL)
            *i_x = p0_x + (t * s1_x);
        if (i_y != NULL)
            *i_y = p0_y + (t * s1_y);
        return 1;
    }

    return 0; // No collision
}
bool intersection(Point2f o1, Point2f p1, Point2f o2, Point2f p2,
                      Point2f &r)
{
    Point2f x = o2 - o1;
    Point2f d1 = p1 - o1;
    Point2f d2 = p2 - o2;

    float cross = d1.x*d2.y - d1.y*d2.x;
    if (abs(cross) < /*EPS*/1e-8)
        return false;

    double t1 = (x.x * d2.y - x.y * d2.x)/cross;
    r = o1 + d1 * t1;
    return true;
}
int count =0;
bool isTemporal ,isNasal,isInferior,isSuperior;
void computeMask(CvPoint point1,CvPoint point2,CvPoint point3,IplImage* srcImg, IplImage* destImg,int val)
{
	cvSetZero(tempMask_float);
	cvSetZero(tempMask);
	cvLine(tempMask_float,point1,point2,cvScalarAll(255),1,CV_AA);
	cvLine(tempMask_float,point1,point3,cvScalarAll(255),1,CV_AA);
	switch(val)
	{
	    case 1 :
		{
			cvLine(tempMask_float,point2,cvPoint(point2.x,0),cvScalarAll(255),1,CV_AA);
			cvLine(tempMask_float,point3,cvPoint(point3.x,0),cvScalarAll(255),1,CV_AA);
			cvLine(tempMask_float,cvPoint(point2.x,0),cvPoint(point3.x,0),cvScalarAll(255),10,CV_AA);


		  break;
		}
		case 2 :
		{
			cvLine(tempMask_float,point2,cvPoint(point2.x,tempMask_float->height),cvScalarAll(255),1,CV_AA);
			cvLine(tempMask_float,point3,cvPoint(point3.x,tempMask_float->height),cvScalarAll(255),1,CV_AA);
			cvLine(tempMask_float,cvPoint(point2.x,tempMask_float->height),cvPoint(point3.x,tempMask_float->height),cvScalarAll(255),10,CV_AA);
		  break;
		}
		case 3 :
		{
			cvLine(tempMask_float,point2,cvPoint(0,point2.y),cvScalarAll(255),1,CV_AA);
			cvLine(tempMask_float,point3,cvPoint(0,point3.y),cvScalarAll(255),1,CV_AA);
			cvLine(tempMask_float,cvPoint(0,point2.y),cvPoint(0,point3.y),cvScalarAll(255),10,CV_AA);
		  break;
		}
		case 4 :
		{
			cvLine(tempMask_float,point2,cvPoint(tempMask_float->width,point2.y),cvScalarAll(255),1,CV_AA);
			cvLine(tempMask_float,point3,cvPoint(tempMask_float->width,point3.y),cvScalarAll(255),1,CV_AA);
			cvLine(tempMask_float,cvPoint(tempMask_float->width,point2.y),cvPoint(tempMask_float->width,point3.y),cvScalarAll(255),10,CV_AA);

		  break;
		}
	}

	//cvLine(tempMask_float,point2,point3,cvScalarAll(255),1,CV_AA);
	cvConvertScale(tempMask_float,tempMask);
	CvMemStorage *storage = cvCreateMemStorage(0);
	CvSeq *contours = cvCreateSeq(0, sizeof(CvSeq), sizeof(CvPoint), storage);
	int contourCnt = cvFindContours(tempMask, storage, &contours, sizeof(CvContour), CV_RETR_LIST,
		CV_CHAIN_APPROX_SIMPLE, cvPoint(0,0));
	for ( ; contours!=0; contours = contours->h_next)
	{
		cvDrawContours(tempMask,contours,cvScalarAll(255),cvScalarAll(255),255,-1);
	}

	cvCopy(srcImg,destImg,tempMask);


}

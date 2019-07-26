
#ifndef INCLUDED_LUT
#define INCLUDED_LUT
#include "AssistedFocus_Globals.h"

 class AssistedFocus_LUT
{
public:
CvMat *ASSISTEDFOCUS_realH,*ASSISTEDFOCUS_imagH,*ASSISTEDFOCUS_lut,*ASSISTEDFOCUS_log_Array[6];

 void AssistedFocus_AssignLUTtoMAt();
 void AssistedFocus_ReleaseLUTMAt();
};

#endif 
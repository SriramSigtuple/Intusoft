GNOME.org
GNOME: Git Repository
Home Git Help
index : gimp	
 switch
GNU Image Manipulation Program	C, Python, Scheme
summaryrefslogtreecommitdiffstats	
 
 search
path: root/plug-ins/common/unsharp-mask.c
blob: 9620735c94fa646ddc52db2861af7a48936a6578 (plain)
1
2
3
4
5
6
7
8
9
10
11
12
13
14
15
16
17
18
19
20
21
22
23
24
25
26
27
28
29
30
31
32
33
34
35
36
37
38
39
40
41
42
43
44
45
46
47
48
49
50
51
52
53
54
55
56
57
58
59
60
61
62
63
64
65
66
67
68
69
70
71
72
73
74
75
76
77
78
79
80
81
82
83
84
85
86
87
88
89
90
91
92
93
94
95
96
97
98
99
100
101
102
103
104
105
106
107
108
109
110
111
112
113
114
115
116
117
118
119
120
121
122
123
124
125
126
127
128
129
130
131
132
133
134
135
136
137
138
139
140
141
142
143
144
145
146
147
148
149
150
151
152
153
154
155
156
157
158
159
160
161
162
163
164
165
166
167
168
169
170
171
172
173
174
175
176
177
178
179
180
181
182
183
184
185
186
187
188
189
190
191
192
193
194
195
196
197
198
199
200
201
202
203
204
205
206
207
208
209
210
211
212
213
214
215
216
217
218
219
220
221
222
223
224
225
226
227
228
229
230
231
232
233
234
235
236
237
238
239
240
241
242
243
244
245
246
247
248
249
250
251
252
253
254
255
256
257
258
259
260
261
262
263
264
265
266
267
268
269
270
271
272
273
274
275
276
277
278
279
280
281
282
283
284
285
286
287
288
289
290
291
292
293
294
295
296
297
298
299
300
301
302
303
304
305
306
307
308
309
310
311
312
313
314
315
316
317
318
319
320
321
322
323
324
325
326
327
328
329
330
331
332
333
334
335
336
337
338
339
340
341
342
343
344
345
346
347
348
349
350
351
352
353
354
355
356
357
358
359
360
361
362
363
364
365
366
367
368
369
370
371
372
373
374
375
376
377
378
379
380
381
382
383
384
385
386
387
388
389
390
391
392
393
394
395
396
397
398
399
400
401
402
403
404
405
406
407
408
409
410
411
412
413
414
415
416
417
418
419
420
421
422
423
424
425
426
427
428
429
430
431
432
433
434
435
436
437
438
439
440
441
442
443
444
445
446
447
448
449
450
451
452
453
454
455
456
457
458
459
460
461
462
463
464
465
466
467
468
469
470
471
472
473
474
475
476
477
478
479
480
481
482
483
484
485
486
487
488
489
490
491
492
493
494
495
496
497
498
499
500
501
502
503
504
505
506
507
508
509
510
511
512
513
514
515
516
517
518
519
520
521
522
523
524
525
526
527
528
529
530
531
532
533
534
535
536
537
538
539
540
541
542
543
544
545
546
547
548
549
550
551
552
553
554
555
556
557
558
559
560
561
562
563
564
565
566
567
568
569
570
571
572
573
574
575
576
577
578
579
580
581
582
583
584
585
586
587
588
589
590
591
592
593
594
595
596
597
598
599
600
601
602
603
604
605
606
607
608
609
610
611
612
613
614
615
616
617
618
619
620
621
622
623
624
625
626
627
628
629
630
631
632
633
634
635
636
637
638
639
640
641
642
643
644
645
646
647
648
649
650
651
652
653
654
655
656
657
658
659
660
661
662
663
664
665
666
667
668
669
670
671
672
673
674
675
676
677
678
679
680
681
682
683
684
685
686
687
688
689
690
691
692
693
694
695
696
697
698
699
700
701
702
703
704
705
706
707
708
709
710
711
712
713
714
715
716
717
718
719
720
721
722
723
724
725
726
727
728
729
730
731
732
733
734
735
736
737
738
739
740
741
742
743
744
745
746
747
748
749
750
751
752
753
754
755
756
757
758
759
760
761
762
763
764
765
766
767
768
769
770
771
772
773
774
775
776
777
778
779
780
781
782
783
784
785
786
787
788
789
790
791
792
793
794
795
796
797
798
799
800
801
802
803
804
805
806
807
808
809
810
811
812
813
814
815
816
817
818
819
820
821
822
823
824
825
826
827
828
829
830
831
832
833
834
835
836
837
838
839
840
841
842
843
844
845
846
847
848
849
850
851
852
853
854
855
856
857
858
859
860
861
862
863
864
865
866
867
868
869
870
871
872
873
874
875
876
877
878
879
880
881
882
883
884
885
886
887
888
889
890
891
892
893
894
895
896
897
898
899
900
901
902
903
904
905
906
907
908
909
910
911
912
913
914
915
916
917
918
919
920
921
922
923
924
925
926
927
928
929
930
931
932
933
934
935
936
937
938
939
940
941
942
943
944
945
946
947
948
949
950
951
952
953
954
/*
 * Copyright (C) 1999 Winston Chang
 *                    <winstonc@cs.wisc.edu>
 *                    <winston@stdout.org>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

#include "config.h"

#include <stdlib.h>

#include <libgimp/gimp.h>
#include <libgimp/gimpui.h>

#include "libgimp/stdplugins-intl.h"


#define PLUG_IN_PROC    "plug-in-unsharp-mask"
#define PLUG_IN_BINARY  "unsharp-mask"
#define PLUG_IN_ROLE    "gimp-unsharp-mask"

#define SCALE_WIDTH   120
#define ENTRY_WIDTH     5

/* Uncomment this line to get a rough estimate of how long the plug-in
 * takes to run.
 */

/*  #define TIMER  */


typedef struct
{
  gdouble  radius;
  gdouble  amount;
  gint     threshold;
} UnsharpMaskParams;

typedef struct
{
  gboolean  run;
} UnsharpMaskInterface;

/* local function prototypes */
static void      query (void);
static void      run   (const gchar      *name,
                        gint              nparams,
                        const GimpParam  *param,
                        gint             *nreturn_vals,
                        GimpParam       **return_vals);

static void      gaussian_blur_line  (const gdouble  *cmatrix,
                                      const gint      cmatrix_length,
                                      const guchar   *src,
                                      guchar         *dest,
                                      const gint      len,
                                      const gint      bpp);
static void      box_blur_line       (const gint      box_width,
                                      const gint      even_offset,
                                      const guchar   *src,
                                      guchar         *dest,
                                      const gint      len,
                                      const gint      bpp);
static gint      gen_convolve_matrix (gdouble         std_dev,
                                      gdouble       **cmatrix);
static void      unsharp_region      (GimpPixelRgn   *srcPTR,
                                      GimpPixelRgn   *dstPTR,
                                      gint            bpp,
                                      gdouble         radius,
                                      gdouble         amount,
                                      gint            x1,
                                      gint            x2,
                                      gint            y1,
                                      gint            y2,
                                      gboolean        show_progress);

static void      unsharp_mask        (GimpDrawable   *drawable,
                                      gdouble         radius,
                                      gdouble         amount);

static gboolean  unsharp_mask_dialog (GimpDrawable   *drawable);
static void      preview_update      (GimpPreview    *preview,
                                      GimpDrawable   *drawable);


/* create a few globals, set default values */
static UnsharpMaskParams unsharp_params =
{
  5.0, /* default radius    */
  0.5, /* default amount    */
  0    /* default threshold */
};

/* Setting PLUG_IN_INFO */
const GimpPlugInInfo PLUG_IN_INFO =
  {
    NULL,  /* init_proc  */
    NULL,  /* quit_proc  */
    query, /* query_proc */
    run,   /* run_proc   */
  };


MAIN ()

static void
query (void)
{
  static const GimpParamDef args[] =
    {
      { GIMP_PDB_INT32,    "run-mode",  "The run mode { RUN-INTERACTIVE (0), RUN-NONINTERACTIVE (1) }" },
      { GIMP_PDB_IMAGE,    "image",     "(unused)" },
      { GIMP_PDB_DRAWABLE, "drawable",  "Drawable to draw on" },
      { GIMP_PDB_FLOAT,    "radius",    "Radius of gaussian blur (in pixels > 1.0)" },
      { GIMP_PDB_FLOAT,    "amount",    "Strength of effect" },
      { GIMP_PDB_INT32,    "threshold", "Threshold (0-255)" }
    };

  gimp_install_procedure (PLUG_IN_PROC,
                          N_("The most widely useful method for sharpening an image"),
                          "The unsharp mask is a sharpening filter that works "
                          "by comparing using the difference of the image and "
                          "a blurred version of the image.  It is commonly "
                          "used on photographic images, and is provides a much "
                          "more pleasing result than the standard sharpen "
                          "filter.",
                          "Winston Chang <winstonc@cs.wisc.edu>",
                          "Winston Chang",
                          "1999-2009",
                          N_("_Unsharp Mask..."),
                          "GRAY*, RGB*",
                          GIMP_PLUGIN,
                          G_N_ELEMENTS (args), 0,
                          args, NULL);

}

static void
run (const gchar      *name,
     gint              nparams,
     const GimpParam  *param,
     gint             *nreturn_vals,
     GimpParam       **return_vals)
{
  static GimpParam   values[1];
  GimpPDBStatusType  status = GIMP_PDB_SUCCESS;
  GimpDrawable      *drawable;
  GimpRunMode        run_mode;
#ifdef TIMER
  GTimer            *timer = g_timer_new ();
#endif

  run_mode = param[0].data.d_int32;

  *return_vals  = values;
  *nreturn_vals = 1;

  values[0].type          = GIMP_PDB_STATUS;
  values[0].data.d_status = status;

  INIT_I18N ();

  /*
   * Get drawable information...
   */
  drawable = gimp_drawable_get (param[2].data.d_drawable);
  gimp_tile_cache_ntiles (2 * MAX (drawable->width  / gimp_tile_width () + 1 ,
                                   drawable->height / gimp_tile_height () + 1));

  switch (run_mode)
    {
    case GIMP_RUN_INTERACTIVE:
      gimp_get_data (PLUG_IN_PROC, &unsharp_params);
      /* Reset default values show preview unmodified */

      /* initialize pixel regions and buffer */
      if (! unsharp_mask_dialog (drawable))
        return;

      break;

    case GIMP_RUN_NONINTERACTIVE:
      if (nparams != 6)
        {
          status = GIMP_PDB_CALLING_ERROR;
        }
      else
        {
          unsharp_params.radius = param[3].data.d_float;
          unsharp_params.amount = param[4].data.d_float;
          unsharp_params.threshold = param[5].data.d_int32;

          /* make sure there are legal values */
          if ((unsharp_params.radius < 0.0) ||
              (unsharp_params.amount < 0.0))
            status = GIMP_PDB_CALLING_ERROR;
        }
      break;

    case GIMP_RUN_WITH_LAST_VALS:
      gimp_get_data (PLUG_IN_PROC, &unsharp_params);
      break;

    default:
      break;
    }

  if (status == GIMP_PDB_SUCCESS)
    {
      drawable = gimp_drawable_get (param[2].data.d_drawable);

      /* here we go */
      unsharp_mask (drawable, unsharp_params.radius, unsharp_params.amount);

      gimp_displays_flush ();

      /* set data for next use of filter */
      if (run_mode == GIMP_RUN_INTERACTIVE)
        gimp_set_data (PLUG_IN_PROC,
                       &unsharp_params, sizeof (UnsharpMaskParams));

      gimp_drawable_detach(drawable);
      values[0].data.d_status = status;
    }

#ifdef TIMER
  g_printerr ("%f seconds\n", g_timer_elapsed (timer, NULL));
  g_timer_destroy (timer);
#endif
}

/* This function is written as if it is blurring a row of pixels,
 * even though it can operate on colums, too.  There is no difference
 * in the processing of the lines, at least to the blur_line function.
 */
static void
box_blur_line (const gint    box_width,   /* Width of the kernel           */
               const gint    even_offset, /* If even width,
                                             offset to left or right       */
               const guchar *src,         /* Pointer to source buffer      */
               guchar       *dest,        /* Pointer to destination buffer */
               const gint    len,         /* Length of buffer, in pixels   */
               const gint    bpp)         /* Bytes per pixel               */
{
  gint  i;
  gint  lead;    /* This marks the leading edge of the kernel              */
  gint  output;  /* This marks the center of the kernel                    */
  gint  trail;   /* This marks the pixel BEHIND the last 1 in the
                    kernel; it's the pixel to remove from the accumulator. */
  gint  ac[bpp]; /* Accumulator for each channel                           */


  /* The algorithm differs for even and odd-sized kernels.
   * With the output at the center,
   * If odd, the kernel might look like this: 0011100
   * If even, the kernel will either be centered on the boundary between
   * the output and its left neighbor, or on the boundary between the
   * output and its right neighbor, depending on even_lr.
   * So it might be 0111100 or 0011110, where output is on the center
   * of these arrays.
   */
  lead = 0;

  if (box_width % 2)
    /* Odd-width kernel */
    {
      output = lead - (box_width - 1) / 2;
      trail  = lead - box_width;
    }
  else
    /* Even-width kernel. */
    {
      /* Right offset */
      if (even_offset == 1)
        {
          output = lead + 1 - box_width / 2;
          trail  = lead - box_width;
        }
      /* Left offset */
      else if (even_offset == -1)
        {
          output = lead - box_width / 2;
          trail  = lead - box_width;
        }
      /* If even_offset isn't 1 or -1, there's some error. */
      else
        g_assert_not_reached ();
    }

  /* Initialize accumulator */
  for (i = 0; i < bpp; i++)
    ac[i] = 0;

  /* As the kernel moves across the image, it has a leading edge and a
   * trailing edge, and the output is in the middle. */
  while (output < len)
    {
      /* The number of pixels that are both in the image and
       * currently covered by the kernel. This is necessary to
       * handle edge cases. */
      guint coverage = (lead < len ? lead : len-1) - (trail >=0 ? trail : -1);

#ifdef READABLE_BOXBLUR_CODE
/* The code here does the same as the code below, but the code below
 * has been optimized by moving the if statements out of the tight for
 * loop, and is harder to understand.
 * Don't use both this code and the code below. */
      for (i = 0; i < bpp; i++)
        {
          /* If the leading edge of the kernel is still on the image,
           * add the value there to the accumulator. */
          if (lead < len)
            ac[i] += src[bpp * lead + i];

          /* If the trailing edge of the kernel is on the image,
           * subtract the value there from the accumulator. */
          if (trail >= 0)
            ac[i] -= src[bpp * trail + i];

          /* Take the averaged value in the accumulator and store
           * that value in the output. The number of pixels currently
           * stored in the accumulator can be less than the nominal
           * width of the kernel because the kernel can go "over the edge"
           * of the image. */
          if (output >= 0)
            dest[bpp * output + i] = (ac[i] + (coverage >> 1)) / coverage;
        }
#endif

      /* If the leading edge of the kernel is still on the image... */
      if (lead < len)
        {
          /* If the trailing edge of the kernel is on the image. (Since
           * the output is in between the lead and trail, it must be on
           * the image. */
          if (trail >= 0)
            for (i = 0; i < bpp; i++)
              {
                ac[i] += src[bpp * lead + i];
                ac[i] -= src[bpp * trail + i];
                dest[bpp * output + i] = (ac[i] + (coverage >> 1)) / coverage;
              }
          /* If the output is on the image, but the trailing edge isn't yet
           * on the image. */
          else if (output >= 0)
            for (i = 0; i < bpp; i++)
              {
                ac[i] += src[bpp * lead + i];
                dest[bpp * output + i] = (ac[i] + (coverage >> 1)) / coverage;
              }
          /* If leading edge is on the image, but the output and trailing
           * edge aren't yet on the image. */
          else
            for (i = 0; i < bpp; i++)
              ac[i] += src[bpp * lead + i];
        }
      /* If the leading edge has gone off the image, but the output and
       * trailing edge are on the image. (The big loop exits when the
       * output goes off the image. */
      else if (trail >= 0)
        {
          for (i = 0; i < bpp; i++)
            {
              ac[i] -= src[bpp * trail + i];
              dest[bpp * output + i] = (ac[i] + (coverage >> 1)) / coverage;
            }
        }
      /* Leading has gone off the image and trailing isn't yet in it
       * (small image) */
      else if (output >= 0)
        {
          for (i = 0; i < bpp; i++)
            dest[bpp * output + i] = (ac[i] + (coverage >> 1)) / coverage;
        }

      lead++;
      output++;
      trail++;
    }
}


/* This function is written as if it is blurring a column at a time,
 * even though it can operate on rows, too.  There is no difference
 * in the processing of the lines, at least to the blur_line function.
 */
static void
gaussian_blur_line (const gdouble *cmatrix,
                    const gint     cmatrix_length,
                    const guchar  *src,
                    guchar        *dest,
                    const gint     len,
                    const gint     bpp)
{
  const guchar  *src_p;
  const guchar  *src_p1;
  const gint     cmatrix_middle = cmatrix_length / 2;
  gint           row;
  gint           i, j;

  /* This first block is the same as the optimized version --
   * it is only used for very small pictures, so speed isn't a
   * big concern.
   */
  if (cmatrix_length > len)
    {
      for (row = 0; row < len; row++)
        {
          /* find the scale factor */
          gdouble scale = 0;

          for (j = 0; j < len; j++)
            {
              /* if the index is in bounds, add it to the scale counter */
              if (j + cmatrix_middle - row >= 0 &&
                  j + cmatrix_middle - row < cmatrix_length)
                scale += cmatrix[j];
            }

          src_p = src;

          for (i = 0; i < bpp; i++)
            {
              gdouble sum = 0;

              src_p1 = src_p++;

              for (j = 0; j < len; j++)
                {
                  if (j + cmatrix_middle - row >= 0 &&
                      j + cmatrix_middle - row < cmatrix_length)
                    sum += *src_p1 * cmatrix[j];

                  src_p1 += bpp;
                }

              *dest++ = (guchar) ROUND (sum / scale);
            }
        }
    }
  else
    {
      /* for the edge condition, we only use available info and scale to one */
      for (row = 0; row < cmatrix_middle; row++)
        {
          /* find scale factor */
          gdouble scale = 0;

          for (j = cmatrix_middle - row; j < cmatrix_length; j++)
            scale += cmatrix[j];

          src_p = src;

          for (i = 0; i < bpp; i++)
            {
              gdouble sum = 0;

              src_p1 = src_p++;

              for (j = cmatrix_middle - row; j < cmatrix_length; j++)
                {
                  sum += *src_p1 * cmatrix[j];
                  src_p1 += bpp;
                }

              *dest++ = (guchar) ROUND (sum / scale);
            }
        }

      /* go through each pixel in each col */
      for (; row < len - cmatrix_middle; row++)
        {
          src_p = src + (row - cmatrix_middle) * bpp;

          for (i = 0; i < bpp; i++)
            {
              gdouble sum = 0;

              src_p1 = src_p;

              for (j = 0; j < cmatrix_length; j++)
                {
                  sum += cmatrix[j] * *src_p1;
                  src_p1 += bpp;
                }

              src_p++;
              *dest++ = (guchar) ROUND (sum);
            }
        }

      /* for the edge condition, we only use available info and scale to one */
      for (; row < len; row++)
        {
          /* find scale factor */
          gdouble scale = 0;

          for (j = 0; j < len - row + cmatrix_middle; j++)
            scale += cmatrix[j];

          src_p = src + (row - cmatrix_middle) * bpp;

          for (i = 0; i < bpp; i++)
            {
              gdouble sum = 0;

              src_p1 = src_p++;

              for (j = 0; j < len - row + cmatrix_middle; j++)
                {
                  sum += *src_p1 * cmatrix[j];
                  src_p1 += bpp;
                }

              *dest++ = (guchar) ROUND (sum / scale);
            }
        }
    }
}

static void
unsharp_mask (GimpDrawable *drawable,
              gdouble       radius,
              gdouble       amount)
{
  GimpPixelRgn srcPR, destPR;
  gint         x1, y1, x2, y2;

  /* initialize pixel regions */
  gimp_pixel_rgn_init (&srcPR, drawable,
                       0, 0, drawable->width, drawable->height, FALSE, FALSE);
  gimp_pixel_rgn_init (&destPR, drawable,
                       0, 0, drawable->width, drawable->height, TRUE, TRUE);

  /* Get the input */
  gimp_drawable_mask_bounds (drawable->drawable_id, &x1, &y1, &x2, &y2);

  unsharp_region (&srcPR, &destPR, drawable->bpp,
                  radius, amount,
                  x1, x2, y1, y2,
                  TRUE);

  gimp_drawable_flush (drawable);
  gimp_drawable_merge_shadow (drawable->drawable_id, TRUE);
  gimp_drawable_update (drawable->drawable_id, x1, y1, x2 - x1, y2 - y1);
}

/* Perform an unsharp mask on the region, given a source region, dest.
 * region, width and height of the regions, and corner coordinates of
 * a subregion to act upon.  Everything outside the subregion is unaffected.
 */
static void
unsharp_region (GimpPixelRgn *srcPR,
                GimpPixelRgn *destPR,
                gint          bpp,
                gdouble       radius, /* Radius, AKA standard deviation */
                gdouble       amount,
                gint          x1,
                gint          x2,
                gint          y1,
                gint          y2,
                gboolean      show_progress)
{
  guchar     *src;                /* Temporary copy of source row/col      */
  guchar     *dest;               /* Temporary copy of destination row/col */
  const gint  width   = x2 - x1;
  const gint  height  = y2 - y1;
  gdouble    *cmatrix = NULL;     /* Convolution matrix (for gaussian)     */
  gint        cmatrix_length = 0;
  gint        row, col;           /* Row, column counters                  */
  const gint  threshold = unsharp_params.threshold;
  gboolean    box_blur;           /* If we want to use a three pass box
                                     blur instead of a gaussian blur       */
  gint        box_width = 0;

  if (show_progress)
    gimp_progress_init (_("Blurring"));

  /* If the radius is less than 10, use a true gaussian kernel.  This
   * is slower, but more accurate and allows for finer adjustments.
   * Otherwise use a three-pass box blur; this is much faster but it
   * isn't a perfect approximation, and it only allows radius
   * increments of about 0.42.
   */
  if (radius < 10)
    {
      box_blur = FALSE;
      /* If true gaussian, generate convolution matrix
         and make sure it's smaller than each dimension */
      cmatrix_length = gen_convolve_matrix (radius, &cmatrix);
    }
  else
    {
      box_blur = TRUE;
      /* Three box blurs of this width approximate a gaussian */
      box_width = ROUND (radius * 3 * sqrt (2 * G_PI) / 4);
    }

  /* Allocate buffers temporary copies of a row/column */
  src  = g_new (guchar, MAX (width, height) * bpp);
  dest = g_new (guchar, MAX (width, height) * bpp);

  /* Blur the rows */
  for (row = 0; row < height; row++)
    {
      gimp_pixel_rgn_get_row (srcPR, src, x1, y1 + row, width);

      if (box_blur)
        {
          /* Odd-width box blur: repeat 3 times, centered on output pixel.
           * Swap back and forth between the buffers. */
          if (box_width % 2)
            {
              box_blur_line (box_width, 0, src, dest, width, bpp);
              box_blur_line (box_width, 0, dest, src, width, bpp);
              box_blur_line (box_width, 0, src, dest, width, bpp);
            }
          /* Even-width box blur:
           * This method is suggested by the specification for SVG.
           * One pass with width n, centered between output and right pixel
           * One pass with width n, centered between output and left pixel
           * One pass with width n+1, centered on output pixel
           * Swap back and forth between buffers.
           */
          else
            {
              box_blur_line (box_width,  -1, src, dest, width, bpp);
              box_blur_line (box_width,   1, dest, src, width, bpp);
              box_blur_line (box_width+1, 0, src, dest, width, bpp);
            }
        }
      else
        {
          /* Gaussian blur */
          gaussian_blur_line (cmatrix, cmatrix_length, src, dest, width, bpp);
        }

      gimp_pixel_rgn_set_row (destPR, dest, x1, y1 + row, width);

      if (show_progress && row % 64 == 0)
        gimp_progress_update ((gdouble) row / (3 * height));
    }

  /* Blur the cols. Essentially same as above. */
  for (col = 0; col < width; col++)
    {
      gimp_pixel_rgn_get_col (destPR, src, x1 + col, y1, height);

      if (box_blur)
        {
          /* Odd-width box blur */
          if (box_width % 2)
            {
              box_blur_line (box_width, 0, src, dest, height, bpp);
              box_blur_line (box_width, 0, dest, src, height, bpp);
              box_blur_line (box_width, 0, src, dest, height, bpp);
            }
          /* Even-width box blur */
          else
            {
              box_blur_line (box_width,  -1, src, dest, height, bpp);
              box_blur_line (box_width,   1, dest, src, height, bpp);
              box_blur_line (box_width+1, 0, src, dest, height, bpp);
            }
        }
      else
        {
          /* Gaussian blur */
          gaussian_blur_line (cmatrix, cmatrix_length, src, dest,height, bpp);
        }

      gimp_pixel_rgn_set_col (destPR, dest, x1 + col, y1, height);

      if (show_progress && col % 64 == 0)
        gimp_progress_update ((gdouble) col / (3 * width) + 0.33);
    }

  if (show_progress)
    gimp_progress_set_text (_("Merging"));

  /* merge the source and destination (which currently contains
     the blurred version) images */
  for (row = 0; row < height; row++)
    {
      const guchar *s = src;
      guchar       *d = dest;
      gint          u, v;

      /* get source row */
      gimp_pixel_rgn_get_row (srcPR, src, x1, y1 + row, width);

      /* get dest row */
      gimp_pixel_rgn_get_row (destPR, dest, x1, y1 + row, width);

      /* combine the two */
      for (u = 0; u < width; u++)
        {
          for (v = 0; v < bpp; v++)
            {
              gint value;
              gint diff = *s - *d;

              /* do tresholding */
              if (abs (2 * diff) < threshold)
                diff = 0;

              value = *s++ + amount * diff;
              *d++ = CLAMP (value, 0, 255);
            }
        }

      if (show_progress && row % 64 == 0)
        gimp_progress_update ((gdouble) row / (3 * height) + 0.67);

      gimp_pixel_rgn_set_row (destPR, dest, x1, y1 + row, width);
    }

  if (show_progress)
    gimp_progress_update (1.0);

  g_free (dest);
  g_free (src);
  g_free (cmatrix);
}

/* generates a 1-D convolution matrix to be used for each pass of
 * a two-pass gaussian blur.  Returns the length of the matrix.
 */
static gint
gen_convolve_matrix (gdouble   radius,
                     gdouble **cmatrix_p)
{
  gdouble *cmatrix;
  gdouble  std_dev;
  gdouble  sum;
  gint     matrix_length;
  gint     i, j;

  /* we want to generate a matrix that goes out a certain radius
   * from the center, so we have to go out ceil(rad-0.5) pixels,
   * including the center pixel.  Of course, that's only in one direction,
   * so we have to go the same amount in the other direction, but not count
   * the center pixel again.  So we double the previous result and subtract
   * one.
   * The radius parameter that is passed to this function is used as
   * the standard deviation, and the radius of effect is the
   * standard deviation * 2.  It's a little confusing.
   */
  radius = fabs (radius) + 1.0;

  std_dev = radius;
  radius = std_dev * 2;

  /* go out 'radius' in each direction */
  matrix_length = 2 * ceil (radius - 0.5) + 1;
  if (matrix_length <= 0)
    matrix_length = 1;

  *cmatrix_p = g_new (gdouble, matrix_length);
  cmatrix = *cmatrix_p;

  /*  Now we fill the matrix by doing a numeric integration approximation
   * from -2*std_dev to 2*std_dev, sampling 50 points per pixel.
   * We do the bottom half, mirror it to the top half, then compute the
   * center point.  Otherwise asymmetric quantization errors will occur.
   *  The formula to integrate is e^-(x^2/2s^2).
   */

  /* first we do the top (right) half of matrix */
  for (i = matrix_length / 2 + 1; i < matrix_length; i++)
    {
      gdouble base_x = i - (matrix_length / 2) - 0.5;

      sum = 0;
      for (j = 1; j <= 50; j++)
        {
          gdouble r = base_x + 0.02 * j;

          if (r <= radius)
            sum += exp (- SQR (r) / (2 * SQR (std_dev)));
        }

      cmatrix[i] = sum / 50;
    }

  /* mirror the thing to the bottom half */
  for (i = 0; i <= matrix_length / 2; i++)
    cmatrix[i] = cmatrix[matrix_length - 1 - i];

  /* find center val -- calculate an odd number of quanta to make it
   * symmetric, even if the center point is weighted slightly higher
   * than others.
   */
  sum = 0;
  for (j = 0; j <= 50; j++)
    sum += exp (- SQR (- 0.5 + 0.02 * j) / (2 * SQR (std_dev)));

  cmatrix[matrix_length / 2] = sum / 51;

  /* normalize the distribution by scaling the total sum to one */
  sum = 0;
  for (i = 0; i < matrix_length; i++)
    sum += cmatrix[i];

  for (i = 0; i < matrix_length; i++)
    cmatrix[i] = cmatrix[i] / sum;

  return matrix_length;
}

static gboolean
unsharp_mask_dialog (GimpDrawable *drawable)
{
  GtkWidget *dialog;
  GtkWidget *main_vbox;
  GtkWidget *preview;
  GtkWidget *table;
  GtkObject *adj;
  gboolean   run;

  gimp_ui_init (PLUG_IN_BINARY, TRUE);

  dialog = gimp_dialog_new (_("Unsharp Mask"), PLUG_IN_ROLE,
                            NULL, 0,
                            gimp_standard_help_func, PLUG_IN_PROC,

                            GTK_STOCK_CANCEL, GTK_RESPONSE_CANCEL,
                            GTK_STOCK_OK,     GTK_RESPONSE_OK,

                            NULL);

  gtk_dialog_set_alternative_button_order (GTK_DIALOG (dialog),
                                           GTK_RESPONSE_OK,
                                           GTK_RESPONSE_CANCEL,
                                           -1);

  gimp_window_set_transient (GTK_WINDOW (dialog));

  main_vbox = gtk_box_new (GTK_ORIENTATION_VERTICAL, 12);
  gtk_container_set_border_width (GTK_CONTAINER (main_vbox), 12);
  gtk_box_pack_start (GTK_BOX (gtk_dialog_get_content_area (GTK_DIALOG (dialog))),
                      main_vbox, TRUE, TRUE, 0);
  gtk_widget_show (main_vbox);

  preview = gimp_drawable_preview_new_from_drawable_id (drawable->drawable_id);
  gtk_box_pack_start (GTK_BOX (main_vbox), preview, TRUE, TRUE, 0);
  gtk_widget_show (preview);

  g_signal_connect (preview, "invalidated",
                    G_CALLBACK (preview_update),
                    drawable);

  table = gtk_table_new (3, 3, FALSE);
  gtk_table_set_col_spacings (GTK_TABLE (table), 6);
  gtk_table_set_row_spacings (GTK_TABLE (table), 6);
  gtk_box_pack_start (GTK_BOX (main_vbox), table, FALSE, FALSE, 0);
  gtk_widget_show (table);

  adj = gimp_scale_entry_new (GTK_TABLE (table), 0, 0,
                              _("_Radius:"), SCALE_WIDTH, ENTRY_WIDTH,
                              unsharp_params.radius, 0.1, 500.0, 0.1, 1.0, 1,
                              TRUE, 0, 0,
                              NULL, NULL);

  g_signal_connect (adj, "value-changed",
                    G_CALLBACK (gimp_double_adjustment_update),
                    &unsharp_params.radius);
  g_signal_connect_swapped (adj, "value-changed",
                            G_CALLBACK (gimp_preview_invalidate),
                            preview);

  adj = gimp_scale_entry_new (GTK_TABLE (table), 0, 1,
                              _("_Amount:"), SCALE_WIDTH, ENTRY_WIDTH,
                              unsharp_params.amount, 0.0, 10.0, 0.01, 0.1, 2,
                              TRUE, 0, 0,
                              NULL, NULL);

  g_signal_connect (adj, "value-changed",
                    G_CALLBACK (gimp_double_adjustment_update),
                    &unsharp_params.amount);
  g_signal_connect_swapped (adj, "value-changed",
                            G_CALLBACK (gimp_preview_invalidate),
                            preview);

  adj = gimp_scale_entry_new (GTK_TABLE (table), 0, 2,
                              _("_Threshold:"), SCALE_WIDTH, ENTRY_WIDTH,
                              unsharp_params.threshold,
                              0.0, 255.0, 1.0, 10.0, 0,
                              TRUE, 0, 0,
                              NULL, NULL);

  g_signal_connect (adj, "value-changed",
                    G_CALLBACK (gimp_int_adjustment_update),
                    &unsharp_params.threshold);
  g_signal_connect_swapped (adj, "value-changed",
                            G_CALLBACK (gimp_preview_invalidate),
                            preview);

  gtk_widget_show (dialog);

  run = (gimp_dialog_run (GIMP_DIALOG (dialog)) == GTK_RESPONSE_OK);

  gtk_widget_destroy (dialog);

  return run;
}

static void
preview_update (GimpPreview  *preview,
                GimpDrawable *drawable)
{
  gint          x1, x2;
  gint          y1, y2;
  gint          x, y;
  gint          width, height;
  gint          border;
  GimpPixelRgn  srcPR;
  GimpPixelRgn  destPR;

  gimp_pixel_rgn_init (&srcPR, drawable,
                       0, 0, drawable->width, drawable->height, FALSE, FALSE);
  gimp_pixel_rgn_init (&destPR, drawable,
                       0, 0, drawable->width, drawable->height, TRUE, TRUE);

  gimp_preview_get_position (preview, &x, &y);
  gimp_preview_get_size (preview, &width, &height);

  /* enlarge the region to avoid artefacts at the edges of the preview */
  border = 2.0 * unsharp_params.radius + 0.5;
  x1 = MAX (0, x - border);
  y1 = MAX (0, y - border);
  x2 = MIN (x + width  + border, drawable->width);
  y2 = MIN (y + height + border, drawable->height);

  unsharp_region (&srcPR, &destPR, drawable->bpp,
                  unsharp_params.radius, unsharp_params.amount,
                  x1, x2, y1, y2,
                  FALSE);

  gimp_pixel_rgn_init (&destPR, drawable, x, y, width, height, FALSE, TRUE);
  gimp_drawable_preview_draw_region (GIMP_DRAWABLE_PREVIEW (preview), &destPR);
}
The GNOME Project
About Us
Get Involved
Teams
Support GNOME
Contact Us
The GNOME Foundation
Resources
Developer Center
Documentation
Wiki
Mailing Lists
IRC Channels
Bug Tracker
Development Code
Build Tool
News
Press Releases
Latest Release
Planet GNOME
Development News
Identi.ca
Twitter
Copyright © 2004–2015, The GNOME Project.

Hosted by Red Hat. Powered by cgit.
To follow the commits, subscribe to commits-list. (can be limited to specific modules)
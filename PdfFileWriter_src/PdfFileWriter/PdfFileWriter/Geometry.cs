﻿/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	Geometry
//	Double precision drawing support functions.
//
//	Granotech Limited
//	Author: Uzi Granot
//	Version: 1.0
//	Date: April 1, 2013
//	Copyright (C) 2013-2016 Granotech Limited. All Rights Reserved
//
//	PdfFileWriter C# class library and TestPdfFileWriter test/demo
//  application are free software.
//	They is distributed under the Code Project Open License (CPOL).
//	The document PdfFileWriterReadmeAndLicense.pdf contained within
//	the distribution specify the license agreement and other
//	conditions and notes. You must read this document and agree
//	with the conditions specified in order to use this software.
//
//	For version history please refer to PdfDocument.cs
//
/////////////////////////////////////////////////////////////////////

using System;

namespace PdfFileWriter
{
/////////////////////////////////////////////////////////////////////
/// <summary>
/// Point in double precision class
/// </summary>
/////////////////////////////////////////////////////////////////////
public class PointD
	{
	/// <summary>
	/// Gets or sets X
	/// </summary>
	public double	X {get; set;}

	/// <summary>
	/// Gets or sets Y
	/// </summary>
	public double	Y {get; set;}

	/// <summary>
	/// PointD copy constructor
	/// </summary>
	/// <param name="Other">Other point</param>
	public PointD
			(
			PointD Other
			)
		{
		this.X = Other.X;
		this.Y = Other.Y;
		return;
		}

	/// <summary>
	/// PointD constructor
	/// </summary>
	/// <param name="X">X</param>
	/// <param name="Y">Y</param>
	public PointD
			(
			double	X,
			double	Y
			)
		{
		this.X = X;
		this.Y = Y;
		return;
		}

	/// <summary>
	/// PointD constructor
	/// </summary>
	/// <param name="Center">Center point</param>
	/// <param name="Radius">Radius</param>
	/// <param name="Alpha">Angle</param>
	public PointD
			(
			PointD	Center,
			double	Radius,
			double	Alpha
			)
		{
		this.X = Center.X + Radius * Math.Cos(Alpha);
		this.Y = Center.Y + Radius * Math.Sin(Alpha);
		return;
		}

	/// <summary>
	/// PointD constructor
	/// </summary>
	/// <param name="CenterX">Center X</param>
	/// <param name="CenterY">Center Y</param>
	/// <param name="Radius">Radius</param>
	/// <param name="Alpha">Angle</param>
	public PointD
			(
			double	CenterX,
			double	CenterY,
			double	Radius,
			double	Alpha
			)
		{
		this.X = CenterX + Radius * Math.Cos(Alpha);
		this.Y = CenterY + Radius * Math.Sin(Alpha);
		return;
		}

	/// <summary>
	/// PointD constructor
	/// </summary>
	/// <param name="L1">Line 1</param>
	/// <param name="L2">Line 2</param>
	public PointD
			(
			LineD	L1,
			LineD	L2
			)
		{
		double Denom = L1.DX * L2.DY - L1.DY * L2.DX;
		if(Denom == 0.0)
			{
			X = double.NaN;
			Y = double.NaN;
			return;
			}

		double L1DXY = L1.P2.X * L1.P1.Y - L1.P2.Y * L1.P1.X;
		double L2DXY = L2.P2.X * L2.P1.Y - L2.P2.Y * L2.P1.X;

		X = (L1DXY * L2.DX - L2DXY * L1.DX) / Denom;
		Y = (L1DXY * L2.DY - L2DXY * L1.DY) / Denom;
		return;
		}
	}

/////////////////////////////////////////////////////////////////////
/// <summary>
/// Size in double precision class
/// </summary>
/////////////////////////////////////////////////////////////////////
public class SizeD
	{
	/// <summary>
	/// Width
	/// </summary>
	public double	Width {get; set;}

	/// <summary>
	/// Height
	/// </summary>
	public double	Height {get; set;}

	/// <summary>
	/// Default constructor
	/// </summary>
	public SizeD() {}

	/// <summary>
	/// SizeD constructor
	/// </summary>
	/// <param name="Width">Width</param>
	/// <param name="Height">Height</param>
	public SizeD
			(
			double	Width,
			double	Height
			)
		{
		this.Width = Width;
		this.Height = Height;
		return;
		}
	}

/////////////////////////////////////////////////////////////////////
/// <summary>
/// Line in double precision class
/// </summary>
/////////////////////////////////////////////////////////////////////
public class LineD
	{
	/// <summary>
	/// Gets or sets point 1
	/// </summary>
	public PointD	P1 {get; set;}

	/// <summary>
	/// Gets or sets point 2
	/// </summary>
	public PointD	P2 {get; set;}

	/// <summary>
	/// LineD constructor (two points)
	/// </summary>
	/// <param name="P1">Point 1</param>
	/// <param name="P2">Point 2</param>
	public LineD
			(
			PointD	P1,
			PointD	P2
			)
		{
		this.P1 = P1;
		this.P2 = P2;
		return;
		}

	/// <summary>
	/// LineD constructor (coordinates)
	/// </summary>
	/// <param name="X1">Point1 X</param>
	/// <param name="Y1">Point1 Y</param>
	/// <param name="X2">Point2 X</param>
	/// <param name="Y2">Point2 Y</param>
	public LineD
			(
			double		X1,
			double		Y1,
			double		X2,
			double		Y2
			)
		{
		this.P1 = new PointD(X1, Y1);
		this.P2 = new PointD(X2, Y2);
		return;
		}

	/// <summary>
	/// Delta X
	/// </summary>
	public double DX {get {return(P2.X - P1.X);}}

	/// <summary>
	/// Delta Y
	/// </summary>
	public double DY {get {return(P2.Y - P1.Y);}}

	/// <summary>
	/// Line length
	/// </summary>
	public double Length {get {return(Math.Sqrt(DX * DX + DY * DY));}}
	}

/////////////////////////////////////////////////////////////////////
// Bezier in double precision
/////////////////////////////////////////////////////////////////////

/// <summary>
/// Bezier curve class
/// </summary>
/// <remarks>
/// All dimensions are in double precision.
/// </remarks>
public class BezierD
	{
	/// <summary>
	/// Bezier P1
	/// </summary>
	public PointD	P1 {get; set;}

	/// <summary>
	/// Bezier P2
	/// </summary>
	public PointD	P2 {get; set;}
	/// <summary>
	/// Bezier P3
	/// </summary>
	public PointD	P3 {get; set;}
	/// <summary>
	/// Bezier P4
	/// </summary>
	public PointD	P4 {get; set;}

	/// <summary>
	/// Circle factor
	/// </summary>
	/// <remarks>The circle factor makes Bezier curve to look like a circle.</remarks>
	private static double CircleFactor = (Math.Sqrt(2.0) - 1) / 0.75;

	/// <summary>
	/// Bezier constructor
	/// </summary>
	/// <param name="P1">P1</param>
	/// <param name="P2">P2</param>
	/// <param name="P3">P3</param>
	/// <param name="P4">P4</param>
	public BezierD
			(
			PointD	P1,
			PointD	P2,
			PointD	P3,
			PointD	P4
			)
		{
		this.P1 = P1;
		this.P2 = P2;
		this.P3 = P3;
		this.P4 = P4;
		return;
		}

	/// <summary>
	/// Bezier constructor
	/// </summary>
	/// <param name="X1">P1-X</param>
	/// <param name="Y1">P1-Y</param>
	/// <param name="X2">P2-X</param>
	/// <param name="Y2">P2-Y</param>
	/// <param name="X3">P3-X</param>
	/// <param name="Y3">P3-Y</param>
	/// <param name="X4">P4-X</param>
	/// <param name="Y4">P4-Y</param>
	public BezierD
			(
			double		X1,
			double		Y1,
			double		X2,
			double		Y2,
			double		X3,
			double		Y3,
			double		X4,
			double		Y4
			)
		{
		this.P1 = new PointD(X1, Y1);
		this.P2 = new PointD(X2, Y2);
		this.P3 = new PointD(X3, Y3);
		this.P4 = new PointD(X4, Y4);
		return;
		}

	/// <summary>
	/// Bezier constructor
	/// </summary>
	/// <param name="P1">P1</param>
	/// <param name="Factor2">Factor2</param>
	/// <param name="Alpha2">Alpha2</param>
	/// <param name="Factor3">Factor3</param>
	/// <param name="Alpha3">Alpha3</param>
	/// <param name="P4">P4</param>
	public BezierD
			(
			PointD	P1,
			double	Factor2,
			double	Alpha2,
			double	Factor3,
			double	Alpha3,
			PointD	P4
			)
		{
		// save two end points
		this.P1 = P1;
		this.P4 = P4;

		// distance between end points
		LineD Line = new LineD(P1, P4);
		double Length = Line.Length;
		if(Length == 0)
			{
			P2 = P1;
			P3 = P4;
			return;
			}

		// angle of line between end points
		double Alpha = Math.Atan2(Line.DY, Line.DX);

		this.P2 = new PointD(P1, Factor2 * Length, Alpha + Alpha2);
		this.P3 = new PointD(P4, Factor3 * Length, Alpha + Alpha3);
		return;
		}

	/// <summary>
	/// BezierD constructor from quadratic bezier points
	/// </summary>
	/// <param name="QP1">Quadratic Bezier point 1</param>
	/// <param name="QP2">Quadratic Bezier point 2</param>
	/// <param name="QP3">Quadratic Bezier point 3</param>
	public BezierD
			(
			PointD	QP1,
			PointD	QP2,
			PointD	QP3
			)
		{
		//	Any quadratic spline can be expressed as a cubic (where the cubic term is zero).
		//	The end points of the cubic will be the same as the quadratic's.
		//	CP1 = QP1
		//	CP4 = QP3
		//	The two control points for the cubic are:
		//	CP2 = QP1 + 2/3 *(QP2-QP1)
		//	CP3 = QP3 + 2/3 *(QP2-QP3)
		P1 = new PointD(QP1);
		P2 = new PointD(QP1.X + 2 * (QP2.X - QP1.X) / 3, QP1.Y + 2 * (QP2.Y - QP1.Y) / 3);
		P3 = new PointD(QP3.X + 2 * (QP2.X - QP3.X) / 3, QP3.Y + 2 * (QP2.Y - QP3.Y) / 3);
		P4 = new PointD(QP3);
		return;
		}

	/// <summary>
	/// Bezier first quarter circle
	/// </summary>
	/// <param name="X">Center X</param>
	/// <param name="Y">Center Y</param>
	/// <param name="Radius">Radius</param>
	/// <returns>Bezier curve</returns>
	public static BezierD CircleFirstQuarter
			(
			double		X,
			double		Y,
			double		Radius
			)
		{
		return(new BezierD(X + Radius, Y, X + Radius, Y + CircleFactor * Radius, X + CircleFactor * Radius, Y + Radius, X, Y + Radius));
		}

	/// <summary>
	/// Bezier second quarter circle
	/// </summary>
	/// <param name="X">Center X</param>
	/// <param name="Y">Center Y</param>
	/// <param name="Radius">Radius</param>
	/// <returns>Bezier curve</returns>
	public static BezierD CircleSecondQuarter
			(
			double		X,
			double		Y,
			double		Radius
			)
		{
		return(new BezierD(X, Y + Radius, X - CircleFactor * Radius, Y + Radius, X - Radius, Y + CircleFactor * Radius, X - Radius, Y));
		}

	/// <summary>
	/// Bezier third quarter circle
	/// </summary>
	/// <param name="X">Center X</param>
	/// <param name="Y">Center Y</param>
	/// <param name="Radius">Radius</param>
	/// <returns>Bezier curve</returns>
	public static BezierD CircleThirdQuarter
			(
			double		X,
			double		Y,
			double		Radius
			)
		{
		return(new BezierD(X - Radius, Y, X - Radius, Y - CircleFactor * Radius, X - CircleFactor * Radius, Y - Radius, X, Y - Radius));
		}

	/// <summary>
	/// Bezier fourth quarter circle
	/// </summary>
	/// <param name="X">Center X</param>
	/// <param name="Y">Center Y</param>
	/// <param name="Radius">Radius</param>
	/// <returns>Bezier curve</returns>
	public static BezierD CircleFourthQuarter
			(
			double		X,
			double		Y,
			double		Radius
			)
		{
		return(new BezierD(X, Y - Radius, X +CircleFactor * Radius, Y - Radius, X + Radius, Y - CircleFactor * Radius, X + Radius, Y));
		}

	/// <summary>
	/// Oval first quarter
	/// </summary>
	/// <param name="X">Center X</param>
	/// <param name="Y">Center Y</param>
	/// <param name="Width">Width</param>
	/// <param name="Height">Height</param>
	/// <returns>Bezier curve</returns>
	public static BezierD OvalFirstQuarter
			(
			double		X,
			double		Y,
			double		Width,
			double		Height
			)
		{
		return(new BezierD(X + Width, Y, X + Width, Y + CircleFactor * Height, X + CircleFactor * Width, Y + Height, X, Y + Height));
		}

	/// <summary>
	/// Oval second quarter
	/// </summary>
	/// <param name="X">Center X</param>
	/// <param name="Y">Center Y</param>
	/// <param name="Width">Width</param>
	/// <param name="Height">Height</param>
	/// <returns>Bezier curve</returns>
	public static BezierD OvalSecondQuarter
			(
			double		X,
			double		Y,
			double		Width,
			double		Height
			)
		{
		return(new BezierD(X, Y + Height, X - CircleFactor * Width, Y + Height, X - Width, Y + CircleFactor * Height, X - Width, Y));
		}

	/// <summary>
	/// Oval third quarter
	/// </summary>
	/// <param name="X">Center X</param>
	/// <param name="Y">Center Y</param>
	/// <param name="Width">Width</param>
	/// <param name="Height">Height</param>
	/// <returns>Bezier curve</returns>
	public static BezierD OvalThirdQuarter
			(
			double		X,
			double		Y,
			double		Width,
			double		Height
			)
		{
		return(new BezierD(X - Width, Y, X - Width, Y - CircleFactor * Height, X - CircleFactor * Width, Y - Height, X, Y - Height));
		}

	/// <summary>
	/// Oval fourth quarter circle
	/// </summary>
	/// <param name="X">Center X</param>
	/// <param name="Y">Center Y</param>
	/// <param name="Width">Width</param>
	/// <param name="Height">Height</param>
	/// <returns>Bezier curve</returns>
	public static BezierD OvalFourthQuarter
			(
			double		X,
			double		Y,
			double		Width,
			double		Height
			)
		{
		return(new BezierD(X, Y - Height, X +CircleFactor * Width, Y - Height, X + Width, Y - CircleFactor * Height, X + Width, Y));
		}
	}
}

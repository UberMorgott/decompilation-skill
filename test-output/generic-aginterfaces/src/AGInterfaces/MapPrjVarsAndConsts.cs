using System;

namespace AGInterfaces;

internal class MapPrjVarsAndConsts
{
	private double a_axis;

	private double a_axis_pi_180;

	private double b_axis;

	private double f;

	private double e1;

	private double e2;

	private double e2_;

	private const double k0 = 0.9996;

	private const double FE = 500000.0;

	private double M1;

	private double M2;

	private double M4;

	private double M6;

	private double N2;

	private double N4;

	private double N6;

	private double N8;

	public MapPrjVarsAndConsts()
	{
		setConsts(6378137.0, 6356752.3142);
	}

	public MapPrjVarsAndConsts(double a, double b)
	{
		setConsts(a, b);
	}

	private void setConsts(double a, double b)
	{
		a_axis = a;
		a_axis_pi_180 = a * Math.PI / 180.0;
		b_axis = b;
		f = (a_axis - b_axis) / a_axis;
		e1 = 0.0818191909289062;
		e2 = f * (2.0 - f);
		e2_ = e2 / (1.0 - e2);
		double num = e2 * e2;
		double num2 = num * e2;
		double num3 = num2 * e2;
		M1 = 1.0 - e2 / 4.0 - 3.0 * num / 64.0 - 5.0 * num2 / 256.0;
		M2 = 3.0 * e2 / 8.0 + 3.0 * num / 32.0 + 45.0 * num2 / 1024.0;
		M4 = 15.0 * num / 256.0 + 45.0 * num2 / 1024.0;
		M6 = 35.0 * num2 / 3072.0;
		N2 = e2 / 2.0 + 5.0 * num / 24.0 + num2 / 12.0 + 13.0 * num3 / 360.0;
		N4 = 7.0 * num / 48.0 + 29.0 * num2 / 240.0 + 811.0 * num3 / 11520.0;
		N6 = 7.0 * num2 / 120.0 + 81.0 * num3 / 1120.0;
		N8 = 4279.0 * num3 / 161280.0;
	}

	public void setVars(double Phi, out double cosPhi, out double tanPhi, out double tanPhi_2, out double ln_tanPhi_2, out double ln_tanPhi_2_pow, out double M, out double v)
	{
		Phi *= Math.PI / 180.0;
		cosPhi = Math.Cos(Phi);
		if (cosPhi != 0.0)
		{
			double num = Math.Sin(Phi);
			tanPhi = num / cosPhi;
			tanPhi_2 = Math.Tan(Phi / 2.0 + Math.PI / 4.0);
			M = a_axis * (M1 * Phi - M2 * Math.Sin(2.0 * Phi) + M4 * Math.Sin(4.0 * Phi) - M6 * Math.Sin(6.0 * Phi));
			double num2 = 1.0 - e2 * num * num;
			v = ((num2 > 0.0) ? (a_axis / Math.Sqrt(num2)) : 0.0);
			num *= e1;
			if (tanPhi_2 != 0.0)
			{
				ln_tanPhi_2 = Math.Log(tanPhi_2);
				ln_tanPhi_2_pow = Math.Log(tanPhi_2 * Math.Pow((1.0 - num) / (1.0 + num), e1 / 2.0));
			}
			else
			{
				ln_tanPhi_2 = (ln_tanPhi_2_pow = 0.0);
			}
		}
		else
		{
			tanPhi = (tanPhi_2 = (ln_tanPhi_2 = (ln_tanPhi_2_pow = (M = (v = 0.0)))));
		}
	}

	public void getUTMCoords(double Phi, double cosPhi, double tanPhi, double M, double v, double Lambda, double Meridian, out double easting, out double northing)
	{
		Lambda *= Math.PI / 180.0;
		double num = (Lambda - Meridian) * cosPhi;
		double num2 = e2_ * cosPhi * cosPhi;
		double num3 = tanPhi * tanPhi;
		double num4 = num * num;
		double num5 = num4 * num;
		double num6 = num5 * num;
		double num7 = num6 * num;
		double num8 = num7 * num;
		double num9 = num3 * num3;
		double num10 = num2 * num2;
		easting = 500000.0 + 0.9996 * v * (num + (1.0 - num3 + num2) * num5 / 6.0 + (5.0 - 18.0 * num3 + num9 + 72.0 * num2 - 58.0 * e2_) * num7 / 120.0);
		northing = (double)((!(Phi >= 0.0)) ? 10000000 : 0) + 0.9996 * (M + v * tanPhi * (num4 / 2.0 + (5.0 - num3 + 9.0 * num2 + 4.0 * num10) * num6 / 24.0 + (61.0 - 58.0 * num3 + num9 + 600.0 * num2 - 330.0 * e2_) * num8 / 720.0));
	}
}

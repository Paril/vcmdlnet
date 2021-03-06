using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCMDL.NET
	{
	//-----------------------------------------------------------------------------
	// A homogeneous row-major 4x4 matrix class.
	//
	// Matrices are concatenated in a left to right order.
	// Multiplies vectors to the left of the matrix.

	public class Matrix4
	{
		public static Matrix4 IDENTITY = new Matrix4(1.0f, 0.0f, 0.0f, 0.0f,
                              0.0f, 1.0f, 0.0f, 0.0f,
                              0.0f, 0.0f, 1.0f, 0.0f,
                              0.0f, 0.0f, 0.0f, 1.0f);
		public float[,] mtx = new float[4,4];

		public Matrix4() {}
		public Matrix4(float m11, float m12, float m13, float m14,
				float m21, float m22, float m23, float m24,
				float m31, float m32, float m33, float m34,
				float m41, float m42, float m43, float m44)
		{
			mtx[0,0] = m11;
			mtx[0,1] = m12;
			mtx[0,2] = m13;
			mtx[0,3] = m14;
			mtx[1,0] = m21;
			mtx[1,1] = m22;
			mtx[1,2] = m23;
			mtx[1,3] = m24;
			mtx[2,0] = m31;
			mtx[2,1] = m32;
			mtx[2,2] = m33;
			mtx[2,3] = m34;
			mtx[3,0] = m41;
			mtx[3,1] = m42;
			mtx[3,2] = m43;
			mtx[3,3] = m44;
		}
		public Matrix4(Matrix4 copy)
		{
			for (int i = 0; i < 4; ++i)
				for (int x = 0; x < 4; ++i)
					mtx[i,x] = copy[i,x];
		}
		~Matrix4() {}

        public float this[int i, int i2]
        {
            get
            {
                return mtx[i,i2];
            }

            set
            {
                mtx[i,i2] = value;
            }
        }

		public static Vector3 operator*(Vector3 lhs, Matrix4 rhs)
		{
			return new Vector3((lhs.x * rhs.mtx[0,0]) + (lhs.y * rhs.mtx[1,0]) + (lhs.z * rhs.mtx[2,0]),
				(lhs.x * rhs.mtx[0,1]) + (lhs.y * rhs.mtx[1,1]) + (lhs.z * rhs.mtx[2,1]),
				(lhs.x * rhs.mtx[0,2]) + (lhs.y * rhs.mtx[1,2]) + (lhs.z * rhs.mtx[2,2]));
		}

		public static Matrix4 operator*(float scalar, Matrix4 rhs)
		{
			return rhs * scalar;
		}

		public static bool operator==(Matrix4 lhs, Matrix4 rhs)
		{
			return VCMath.closeEnough(lhs.mtx[0,0], rhs.mtx[0,0])
				&& VCMath.closeEnough(lhs.mtx[0,1], rhs.mtx[0,1])
				&& VCMath.closeEnough(lhs.mtx[0,2], rhs.mtx[0,2])
				&& VCMath.closeEnough(lhs.mtx[0,3], rhs.mtx[0,3])
				&& VCMath.closeEnough(lhs.mtx[1,0], rhs.mtx[1,0])
				&& VCMath.closeEnough(lhs.mtx[1,1], rhs.mtx[1,1])
				&& VCMath.closeEnough(lhs.mtx[1,2], rhs.mtx[1,2])
				&& VCMath.closeEnough(lhs.mtx[1,3], rhs.mtx[1,3])
				&& VCMath.closeEnough(lhs.mtx[2,0], rhs.mtx[2,0])
				&& VCMath.closeEnough(lhs.mtx[2,1], rhs.mtx[2,1])
				&& VCMath.closeEnough(lhs.mtx[2,2], rhs.mtx[2,2])
				&& VCMath.closeEnough(lhs.mtx[2,3], rhs.mtx[2,3])
				&& VCMath.closeEnough(lhs.mtx[3,0], rhs.mtx[3,0])
				&& VCMath.closeEnough(lhs.mtx[3,1], rhs.mtx[3,1])
				&& VCMath.closeEnough(lhs.mtx[3,2], rhs.mtx[3,2])
				&& VCMath.closeEnough(lhs.mtx[3,3], rhs.mtx[3,3]);
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public static bool operator!=(Matrix4 lhs, Matrix4 rhs)
		{
			return !(lhs == rhs);
		}

		public static Matrix4 operator+(Matrix4 lhs, Matrix4 rhs)
		{
			Matrix4 newMat = new Matrix4(lhs);
			newMat.mtx[0,0] += rhs.mtx[0,0];newMat.mtx[0,1] += rhs.mtx[0,1];newMat.mtx[0,2] += rhs.mtx[0,2];newMat.mtx[0,3] += rhs.mtx[0,3];
			newMat.mtx[1,0] += rhs.mtx[1,0];newMat.mtx[1,1] += rhs.mtx[1,1];newMat.mtx[1,2] += rhs.mtx[1,2];newMat.mtx[1,3] += rhs.mtx[1,3];
			newMat.mtx[2,0] += rhs.mtx[2,0];newMat.mtx[2,1] += rhs.mtx[2,1];newMat.mtx[2,2] += rhs.mtx[2,2];newMat.mtx[2,3] += rhs.mtx[2,3];
			newMat.mtx[3,0] += rhs.mtx[3,0];newMat.mtx[3,1] += rhs.mtx[3,1];newMat.mtx[3,2] += rhs.mtx[3,2];newMat.mtx[3,3] += rhs.mtx[3,3];
			return newMat;
		}

		public static Matrix4 operator-(Matrix4 lhs, Matrix4 rhs)
		{
			Matrix4 newMat = new Matrix4(lhs);
			newMat.mtx[0,0] -= rhs.mtx[0,0];newMat.mtx[0,1] -= rhs.mtx[0,1];newMat.mtx[0,2] -= rhs.mtx[0,2];newMat.mtx[0,3] -= rhs.mtx[0,3];
			newMat.mtx[1,0] -= rhs.mtx[1,0];newMat.mtx[1,1] -= rhs.mtx[1,1];newMat.mtx[1,2] -= rhs.mtx[1,2];newMat.mtx[1,3] -= rhs.mtx[1,3];
			newMat.mtx[2,0] -= rhs.mtx[2,0];newMat.mtx[2,1] -= rhs.mtx[2,1];newMat.mtx[2,2] -= rhs.mtx[2,2];newMat.mtx[2,3] -= rhs.mtx[2,3];
			newMat.mtx[3,0] -= rhs.mtx[3,0];newMat.mtx[3,1] -= rhs.mtx[3,1];newMat.mtx[3,2] -= rhs.mtx[3,2];newMat.mtx[3,3] -= rhs.mtx[3,3];
			return newMat;
		}

		public static Matrix4 operator*(Matrix4 lhs, Matrix4 rhs)
		{
			Matrix4 tmp = new Matrix4();

			// Row 1.
			tmp.mtx[0,0] = (lhs.mtx[0,0] * rhs.mtx[0,0]) + (lhs.mtx[0,1] * rhs.mtx[1,0]) + (lhs.mtx[0,2] * rhs.mtx[2,0]) + (lhs.mtx[0,3] * rhs.mtx[3,0]);
			tmp.mtx[0,1] = (lhs.mtx[0,0] * rhs.mtx[0,1]) + (lhs.mtx[0,1] * rhs.mtx[1,1]) + (lhs.mtx[0,2] * rhs.mtx[2,1]) + (lhs.mtx[0,3] * rhs.mtx[3,1]);
			tmp.mtx[0,2] = (lhs.mtx[0,0] * rhs.mtx[0,2]) + (lhs.mtx[0,1] * rhs.mtx[1,2]) + (lhs.mtx[0,2] * rhs.mtx[2,2]) + (lhs.mtx[0,3] * rhs.mtx[3,2]);
			tmp.mtx[0,3] = (lhs.mtx[0,0] * rhs.mtx[0,3]) + (lhs.mtx[0,1] * rhs.mtx[1,3]) + (lhs.mtx[0,2] * rhs.mtx[2,3]) + (lhs.mtx[0,3] * rhs.mtx[3,3]);

			// Row 2.
			tmp.mtx[1,0] = (lhs.mtx[1,0] * rhs.mtx[0,0]) + (lhs.mtx[1,1] * rhs.mtx[1,0]) + (lhs.mtx[1,2] * rhs.mtx[2,0]) + (lhs.mtx[1,3] * rhs.mtx[3,0]);
			tmp.mtx[1,1] = (lhs.mtx[1,0] * rhs.mtx[0,1]) + (lhs.mtx[1,1] * rhs.mtx[1,1]) + (lhs.mtx[1,2] * rhs.mtx[2,1]) + (lhs.mtx[1,3] * rhs.mtx[3,1]);
			tmp.mtx[1,2] = (lhs.mtx[1,0] * rhs.mtx[0,2]) + (lhs.mtx[1,1] * rhs.mtx[1,2]) + (lhs.mtx[1,2] * rhs.mtx[2,2]) + (lhs.mtx[1,3] * rhs.mtx[3,2]);
			tmp.mtx[1,3] = (lhs.mtx[1,0] * rhs.mtx[0,3]) + (lhs.mtx[1,1] * rhs.mtx[1,3]) + (lhs.mtx[1,2] * rhs.mtx[2,3]) + (lhs.mtx[1,3] * rhs.mtx[3,3]);

			// Row 3.
			tmp.mtx[2,0] = (lhs.mtx[2,0] * rhs.mtx[0,0]) + (lhs.mtx[2,1] * rhs.mtx[1,0]) + (lhs.mtx[2,2] * rhs.mtx[2,0]) + (lhs.mtx[2,3] * rhs.mtx[3,0]);
			tmp.mtx[2,1] = (lhs.mtx[2,0] * rhs.mtx[0,1]) + (lhs.mtx[2,1] * rhs.mtx[1,1]) + (lhs.mtx[2,2] * rhs.mtx[2,1]) + (lhs.mtx[2,3] * rhs.mtx[3,1]);
			tmp.mtx[2,2] = (lhs.mtx[2,0] * rhs.mtx[0,2]) + (lhs.mtx[2,1] * rhs.mtx[1,2]) + (lhs.mtx[2,2] * rhs.mtx[2,2]) + (lhs.mtx[2,3] * rhs.mtx[3,2]);
			tmp.mtx[2,3] = (lhs.mtx[2,0] * rhs.mtx[0,3]) + (lhs.mtx[2,1] * rhs.mtx[1,3]) + (lhs.mtx[2,2] * rhs.mtx[2,3]) + (lhs.mtx[2,3] * rhs.mtx[3,3]);

			// Row 4.
			tmp.mtx[3,0] = (lhs.mtx[3,0] * rhs.mtx[0,0]) + (lhs.mtx[3,1] * rhs.mtx[1,0]) + (lhs.mtx[3,2] * rhs.mtx[2,0]) + (lhs.mtx[3,3] * rhs.mtx[3,0]);
			tmp.mtx[3,1] = (lhs.mtx[3,0] * rhs.mtx[0,1]) + (lhs.mtx[3,1] * rhs.mtx[1,1]) + (lhs.mtx[3,2] * rhs.mtx[2,1]) + (lhs.mtx[3,3] * rhs.mtx[3,1]);
			tmp.mtx[3,2] = (lhs.mtx[3,0] * rhs.mtx[0,2]) + (lhs.mtx[3,1] * rhs.mtx[1,2]) + (lhs.mtx[3,2] * rhs.mtx[2,2]) + (lhs.mtx[3,3] * rhs.mtx[3,2]);
			tmp.mtx[3,3] = (lhs.mtx[3,0] * rhs.mtx[0,3]) + (lhs.mtx[3,1] * rhs.mtx[1,3]) + (lhs.mtx[3,2] * rhs.mtx[2,3]) + (lhs.mtx[3,3] * rhs.mtx[3,3]);

			return tmp;
		}

		public static Matrix4 operator*(Matrix4 lhs, float scalar)
		{
			Matrix4 mat = new Matrix4(lhs);
			mat.mtx[0,0] *= scalar;mat.mtx[0,1] *= scalar;mat.mtx[0,2] *= scalar;mat.mtx[0,3] *= scalar;
			mat.mtx[1,0] *= scalar;mat.mtx[1,1] *= scalar;mat.mtx[1,2] *= scalar;mat.mtx[1,3] *= scalar;
			mat.mtx[2,0] *= scalar;mat.mtx[2,1] *= scalar;mat.mtx[2,2] *= scalar;mat.mtx[2,3] *= scalar;
			mat.mtx[3,0] *= scalar;mat.mtx[3,1] *= scalar;mat.mtx[3,2] *= scalar;mat.mtx[3,3] *= scalar;
			return mat;
		}

		public static Matrix4 operator/(Matrix4 lhs, float scalar)
		{
			Matrix4 mat = new Matrix4(lhs);
			mat.mtx[0,0] /= scalar;mat.mtx[0,1] /= scalar;mat.mtx[0,2] /= scalar;mat.mtx[0,3] /= scalar;
			mat.mtx[1,0] /= scalar;mat.mtx[1,1] /= scalar;mat.mtx[1,2] /= scalar;mat.mtx[1,3] /= scalar;
			mat.mtx[2,0] /= scalar;mat.mtx[2,1] /= scalar;mat.mtx[2,2] /= scalar;mat.mtx[2,3] /= scalar;
			mat.mtx[3,0] /= scalar;mat.mtx[3,1] /= scalar;mat.mtx[3,2] /= scalar;mat.mtx[3,3] /= scalar;
			return mat;
		}

		public float determinant()
		{
			return (mtx[0,0] * mtx[1,1] - mtx[1,0] * mtx[0,1])
				* (mtx[2,2] * mtx[3,3] - mtx[3,2] * mtx[2,3])
				- (mtx[0,0] * mtx[2,1] - mtx[2,0] * mtx[0,1])
				* (mtx[1,2] * mtx[3,3] - mtx[3,2] * mtx[1,3])
				+ (mtx[0,0] * mtx[3,1] - mtx[3,0] * mtx[0,1])
				* (mtx[1,2] * mtx[2,3] - mtx[2,2] * mtx[1,3])
				+ (mtx[1,0] * mtx[2,1] - mtx[2,0] * mtx[1,1])
				* (mtx[0,2] * mtx[3,3] - mtx[3,2] * mtx[0,3])
				- (mtx[1,0] * mtx[3,1] - mtx[3,0] * mtx[1,1])
				* (mtx[0,2] * mtx[2,3] - mtx[2,2] * mtx[0,3])
				+ (mtx[2,0] * mtx[3,1] - mtx[3,0] * mtx[2,1])
				* (mtx[0,2] * mtx[1,3] - mtx[1,2] * mtx[0,3]);
		}

		public void fromAxes(Vector3 x, Vector3 y, Vector3 z)
		{
			mtx[0,0] = x.x;mtx[0,1] = x.y;mtx[0,2] = x.z;mtx[0,3] = 0.0f;
			mtx[1,0] = y.x;mtx[1,1] = y.y;mtx[1,2] = y.z;mtx[1,3] = 0.0f;
			mtx[2,0] = z.x;mtx[2,1] = z.y;mtx[2,2] = z.z;mtx[2,3] = 0.0f;
			mtx[3,0] = 0.0f;mtx[3,1] = 0.0f;mtx[3,2] = 0.0f;mtx[3,3] = 1.0f;
		}

		public void fromAxesTransposed(Vector3 x, Vector3 y, Vector3 z)
		{
			mtx[0,0] = x.x;mtx[0,1] = y.x;mtx[0,2] = z.x;mtx[0,3] = 0.0f;
			mtx[1,0] = x.y;mtx[1,1] = y.y;mtx[1,2] = z.y;mtx[1,3] = 0.0f;
			mtx[2,0] = x.z;mtx[2,1] = y.z;mtx[2,2] = z.z;mtx[2,3] = 0.0f;
			mtx[3,0] = 0.0f;mtx[3,1] = 0.0f;mtx[3,2] = 0.0f;mtx[3,3] = 1.0f;
		}

		public void fromHeadPitchRoll(float headDegrees, float pitchDegrees, float rollDegrees)
		{
			// Constructs a rotation matrix based on a Euler Transform.
			// We use the popular NASA standard airplane convention of 
			// heading-pitch-roll (i.e., RzRxRy).

			headDegrees = VCMath.degreesToRadians(headDegrees);
			pitchDegrees = VCMath.degreesToRadians(pitchDegrees);
			rollDegrees = VCMath.degreesToRadians(rollDegrees);

			float cosH = (float)Math.Cos(headDegrees);
			float cosP = (float)Math.Cos(pitchDegrees);
			float cosR = (float)Math.Cos(rollDegrees);
			float sinH = (float)Math.Sin(headDegrees);
			float sinP = (float)Math.Sin(pitchDegrees);
			float sinR = (float)Math.Sin(rollDegrees);

			mtx[0,0] = cosR * cosH - sinR * sinP * sinH;
			mtx[0,1] = sinR * cosH + cosR * sinP * sinH;
			mtx[0,2] = -cosP * sinH;
			mtx[0,3] = 0.0f;

			mtx[1,0] = -sinR * cosP;
			mtx[1,1] = cosR * cosP;
			mtx[1,2] = sinP;
			mtx[1,3] = 0.0f;

			mtx[2,0] = cosR * sinH + sinR * sinP * cosH;
			mtx[2,1] = sinR * sinH - cosR * sinP * cosH;
			mtx[2,2] = cosP * cosH;
			mtx[2,3] = 0.0f;

			mtx[3,0] = 0.0f;
			mtx[3,1] = 0.0f;
			mtx[3,2] = 0.0f;
			mtx[3,3] = 1.0f;
		}

		public void identity()
		{
			mtx[0,0] = 1.0f;mtx[0,1] = 0.0f;mtx[0,2] = 0.0f;mtx[0,3] = 0.0f;
			mtx[1,0] = 0.0f;mtx[1,1] = 1.0f;mtx[1,2] = 0.0f;mtx[1,3] = 0.0f;
			mtx[2,0] = 0.0f;mtx[2,1] = 0.0f;mtx[2,2] = 1.0f;mtx[2,3] = 0.0f;
			mtx[3,0] = 0.0f;mtx[3,1] = 0.0f;mtx[3,2] = 0.0f;mtx[3,3] = 1.0f;
		}

		public Matrix4 inverse()
		{
			// This method of computing the inverse of a 4x4 matrix is based
			// on a similar function found in Paul Nettle's matrix template
			// class (http://www.fluidstudios.com).
			//
			// If the inverse doesn't exist for this matrix, then the identity
			// matrix will be returned.

			Matrix4 tmp = new Matrix4();
			float d = determinant();

			if (VCMath.closeEnough(d, 0.0f))
				tmp.identity();
			else
			{
				d = 1.0f / d;

				tmp.mtx[0,0] = d * (mtx[1,1] * (mtx[2,2] * mtx[3,3] - mtx[3,2] * mtx[2,3]) + mtx[2,1] * (mtx[3,2] * mtx[1,3] - mtx[1,2] * mtx[3,3]) + mtx[3,1] * (mtx[1,2] * mtx[2,3] - mtx[2,2] * mtx[1,3]));
				tmp.mtx[1,0] = d * (mtx[1,2] * (mtx[2,0] * mtx[3,3] - mtx[3,0] * mtx[2,3]) + mtx[2,2] * (mtx[3,0] * mtx[1,3] - mtx[1,0] * mtx[3,3]) + mtx[3,2] * (mtx[1,0] * mtx[2,3] - mtx[2,0] * mtx[1,3]));
				tmp.mtx[2,0] = d * (mtx[1,3] * (mtx[2,0] * mtx[3,1] - mtx[3,0] * mtx[2,1]) + mtx[2,3] * (mtx[3,0] * mtx[1,1] - mtx[1,0] * mtx[3,1]) + mtx[3,3] * (mtx[1,0] * mtx[2,1] - mtx[2,0] * mtx[1,1]));
				tmp.mtx[3,0] = d * (mtx[1,0] * (mtx[3,1] * mtx[2,2] - mtx[2,1] * mtx[3,2]) + mtx[2,0] * (mtx[1,1] * mtx[3,2] - mtx[3,1] * mtx[1,2]) + mtx[3,0] * (mtx[2,1] * mtx[1,2] - mtx[1,1] * mtx[2,2]));
        
				tmp.mtx[0,1] = d * (mtx[2,1] * (mtx[0,2] * mtx[3,3] - mtx[3,2] * mtx[0,3]) + mtx[3,1] * (mtx[2,2] * mtx[0,3] - mtx[0,2] * mtx[2,3]) + mtx[0,1] * (mtx[3,2] * mtx[2,3] - mtx[2,2] * mtx[3,3]));
				tmp.mtx[1,1] = d * (mtx[2,2] * (mtx[0,0] * mtx[3,3] - mtx[3,0] * mtx[0,3]) + mtx[3,2] * (mtx[2,0] * mtx[0,3] - mtx[0,0] * mtx[2,3]) + mtx[0,2] * (mtx[3,0] * mtx[2,3] - mtx[2,0] * mtx[3,3]));
				tmp.mtx[2,1] = d * (mtx[2,3] * (mtx[0,0] * mtx[3,1] - mtx[3,0] * mtx[0,1]) + mtx[3,3] * (mtx[2,0] * mtx[0,1] - mtx[0,0] * mtx[2,1]) + mtx[0,3] * (mtx[3,0] * mtx[2,1] - mtx[2,0] * mtx[3,1]));
				tmp.mtx[3,1] = d * (mtx[2,0] * (mtx[3,1] * mtx[0,2] - mtx[0,1] * mtx[3,2]) + mtx[3,0] * (mtx[0,1] * mtx[2,2] - mtx[2,1] * mtx[0,2]) + mtx[0,0] * (mtx[2,1] * mtx[3,2] - mtx[3,1] * mtx[2,2]));
        
				tmp.mtx[0,2] = d * (mtx[3,1] * (mtx[0,2] * mtx[1,3] - mtx[1,2] * mtx[0,3]) + mtx[0,1] * (mtx[1,2] * mtx[3,3] - mtx[3,2] * mtx[1,3]) + mtx[1,1] * (mtx[3,2] * mtx[0,3] - mtx[0,2] * mtx[3,3]));
				tmp.mtx[1,2] = d * (mtx[3,2] * (mtx[0,0] * mtx[1,3] - mtx[1,0] * mtx[0,3]) + mtx[0,2] * (mtx[1,0] * mtx[3,3] - mtx[3,0] * mtx[1,3]) + mtx[1,2] * (mtx[3,0] * mtx[0,3] - mtx[0,0] * mtx[3,3]));
				tmp.mtx[2,2] = d * (mtx[3,3] * (mtx[0,0] * mtx[1,1] - mtx[1,0] * mtx[0,1]) + mtx[0,3] * (mtx[1,0] * mtx[3,1] - mtx[3,0] * mtx[1,1]) + mtx[1,3] * (mtx[3,0] * mtx[0,1] - mtx[0,0] * mtx[3,1]));
				tmp.mtx[3,2] = d * (mtx[3,0] * (mtx[1,1] * mtx[0,2] - mtx[0,1] * mtx[1,2]) + mtx[0,0] * (mtx[3,1] * mtx[1,2] - mtx[1,1] * mtx[3,2]) + mtx[1,0] * (mtx[0,1] * mtx[3,2] - mtx[3,1] * mtx[0,2]));
        
				tmp.mtx[0,3] = d * (mtx[0,1] * (mtx[2,2] * mtx[1,3] - mtx[1,2] * mtx[2,3]) + mtx[1,1] * (mtx[0,2] * mtx[2,3] - mtx[2,2] * mtx[0,3]) + mtx[2,1] * (mtx[1,2] * mtx[0,3] - mtx[0,2] * mtx[1,3]));
				tmp.mtx[1,3] = d * (mtx[0,2] * (mtx[2,0] * mtx[1,3] - mtx[1,0] * mtx[2,3]) + mtx[1,2] * (mtx[0,0] * mtx[2,3] - mtx[2,0] * mtx[0,3]) + mtx[2,2] * (mtx[1,0] * mtx[0,3] - mtx[0,0] * mtx[1,3]));
				tmp.mtx[2,3] = d * (mtx[0,3] * (mtx[2,0] * mtx[1,1] - mtx[1,0] * mtx[2,1]) + mtx[1,3] * (mtx[0,0] * mtx[2,1] - mtx[2,0] * mtx[0,1]) + mtx[2,3] * (mtx[1,0] * mtx[0,1] - mtx[0,0] * mtx[1,1]));
				tmp.mtx[3,3] = d * (mtx[0,0] * (mtx[1,1] * mtx[2,2] - mtx[2,1] * mtx[1,2]) + mtx[1,0] * (mtx[2,1] * mtx[0,2] - mtx[0,1] * mtx[2,2]) + mtx[2,0] * (mtx[0,1] * mtx[1,2] - mtx[1,1] * mtx[0,2]));
			}

			return tmp;
		}

		public void orient(Vector3 from, Vector3 to)
		{
			// Creates an orientation matrix that will rotate the vector 'from' 
			// into the vector 'to'. For this method to work correctly, vector
			// 'from' and vector 'to' must both be unit length vectors.
			//
			// The algorithm used is from:
			//   Tomas Moller and John F. Hughes, "Efficiently building a matrix
			//   to rotate one vector to another," Journal of Graphics Tools,
			//   4(4):1-4, 1999.

			float e = from.DotProduct (to);

			if (VCMath.closeEnough(e, 1.0f))
			{
				// Special case where 'from' is equal to 'to'. In other words,
				// the angle between vector 'from' and vector 'to' is zero 
				// degrees. In this case just load the identity matrix.

				identity();
			}
			else if (VCMath.closeEnough(e, -1.0f))
			{
				// Special case where 'from' is directly opposite to 'to'. In
				// other words, the angle between vector 'from' and vector 'to'
				// is 180 degrees. In this case, the following matrix is used:
				//
				// Let:
				//   F = from
				//   S = vector perpendicular to F
				//   U = S X F
				//
				// We want to rotate from (F, U, S) to (-F, U, -S)
				//
				// | -FxFx+UxUx-SxSx  -FxFy+UxUy-SxSy  -FxFz+UxUz-SxSz  0 |
				// | -FxFy+UxUy-SxSy  -FyFy+UyUy-SySy  -FyFz+UyUz-SySz  0 |
				// | -FxFz+UxUz-SxSz  -FyFz+UyUz-SySz  -FzFz+UzUz-SzSz  0 |
				// |       0                 0                0         1 |

				Vector3 side = new Vector3(0.0f, from.z, -from.y);

				if (VCMath.closeEnough(side.DotProduct (side), 0.0f))
					side.Set(-from.z, 0.0f, from.x);

				side.Normalize();

				Vector3 up = side.CrossProduct (from);
				up.Normalize();

				mtx[0,0] = -(from.x * from.x) + (up.x * up.x) - (side.x * side.x);
				mtx[0,1] = -(from.x * from.y) + (up.x * up.y) - (side.x * side.y);
				mtx[0,2] = -(from.x * from.z) + (up.x * up.z) - (side.x * side.z);
				mtx[0,3] = 0.0f;
				mtx[1,0] = -(from.x * from.y) + (up.x * up.y) - (side.x * side.y);
				mtx[1,1] = -(from.y * from.y) + (up.y * up.y) - (side.y * side.y);
				mtx[1,2] = -(from.y * from.z) + (up.y * up.z) - (side.y * side.z);
				mtx[1,3] = 0.0f;
				mtx[2,0] = -(from.x * from.z) + (up.x * up.z) - (side.x * side.z);
				mtx[2,1] = -(from.y * from.z) + (up.y * up.z) - (side.y * side.z);
				mtx[2,2] = -(from.z * from.z) + (up.z * up.z) - (side.z * side.z);
				mtx[2,3] = 0.0f;
				mtx[3,0] = 0.0f;
				mtx[3,1] = 0.0f;
				mtx[3,2] = 0.0f;
				mtx[3,3] = 1.0f;
			}
			else
			{
				// This is the most common case. Creates the rotation matrix:
				//
				//               | E + HVx^2   HVxVy + Vz  HVxVz - Vy  0 |
				// R(from, to) = | HVxVy - Vz  E + HVy^2   HVxVz + Vx  0 |
				//               | HVxVz + Vy  HVyVz - Vx  E + HVz^2   0 |
				//               |     0           0           0       1 |
				//
				// where,
				//   V = from.cross(to)
				//   E = from.dot(to)
				//   H = (1 - E) / V.dot(V)

				Vector3 v = from.CrossProduct (to);
				v.Normalize();

				float h = (1.0f - e) / v.DotProduct (v);

				mtx[0,0] = e + h * v.x * v.x;
				mtx[0,1] = h * v.x * v.y + v.z;
				mtx[0,2] = h * v.x * v.z - v.y;
				mtx[0,3] = 0.0f;

				mtx[1,0] = h * v.x * v.y - v.z;
				mtx[1,1] = e + h * v.y * v.y;
				mtx[1,2] = h * v.x * v.z + v.x;
				mtx[1,3] = 0.0f;

				mtx[2,0] = h * v.x * v.z + v.y;
				mtx[2,1] = h * v.y * v.z - v.x;
				mtx[2,2] = e + h * v.z * v.z;
				mtx[2,3] = 0.0f;

				mtx[3,0] = 0.0f;
				mtx[3,1] = 0.0f;
				mtx[3,2] = 0.0f;
				mtx[3,3] = 1.0f;
			}
		}

		public void rotate(Vector3 axis, float degrees)
		{
			// Creates a rotation matrix about the specified axis.
			// The axis must be a unit vector. The angle must be in degrees.
			//
			// Let u = axis of rotation = (x, y, z)
			//
			//             | x^2(1 - c) + c  xy(1 - c) + zs  xz(1 - c) - ys   0 |
			// Ru(angle) = | yx(1 - c) - zs  y^2(1 - c) + c  yz(1 - c) + xs   0 |
			//             | zx(1 - c) - ys  zy(1 - c) - xs  z^2(1 - c) + c   0 |
			//             |      0              0                0           1 |
			//
			// where,
			//	c = cos(angle)
			//  s = sin(angle)

			degrees = VCMath.degreesToRadians(degrees);

			float x = axis.x;
			float y = axis.y;
			float z = axis.z;
			float c = (float)Math.Cos(degrees);
			float s = (float)Math.Sin(degrees);

			mtx[0,0] = (x * x) * (1.0f - c) + c;
			mtx[0,1] = (x * y) * (1.0f - c) + (z * s);
			mtx[0,2] = (x * z) * (1.0f - c) - (y * s);
			mtx[0,3] = 0.0f;

			mtx[1,0] = (y * x) * (1.0f - c) - (z * s);
			mtx[1,1] = (y * y) * (1.0f - c) + c;
			mtx[1,2] = (y * z) * (1.0f - c) + (x * s);
			mtx[1,3] = 0.0f;

			mtx[2,0] = (z * x) * (1.0f - c) + (y * s);
			mtx[2,1] = (z * y) * (1.0f - c) - (x * s);
			mtx[2,2] = (z * z) * (1.0f - c) + c;
			mtx[2,3] = 0.0f;

			mtx[3,0] = 0.0f;
			mtx[3,1] = 0.0f;
			mtx[3,2] = 0.0f;
			mtx[3,3] = 1.0f;
		}

		public void scale(float sx, float sy, float sz)
		{
			// Creates a scaling matrix.
			//
			//                 | sx   0    0    0 |
			// S(sx, sy, sz) = | 0    sy   0    0 |
			//                 | 0    0    sz   0 |
			//                 | 0    0    0    1 |

			mtx[0,0] = sx;mtx[0,1] = 0.0f;mtx[0,2] = 0.0f;mtx[0,3] = 0.0f;
			mtx[1,0] = 0.0f;mtx[1,1] = sy;mtx[1,2] = 0.0f;mtx[1,3] = 0.0f;
			mtx[2,0] = 0.0f;mtx[2,1] = 0.0f;mtx[2,2] = sz;mtx[2,3] = 0.0f;
			mtx[3, 0] = 0.0f; mtx[3, 1] = 0.0f; mtx[3, 2] = 0.0f; mtx[3, 3] = 1.0f;
		}

		public void toHeadPitchRoll(ref float headDegrees, ref float pitchDegrees, ref float rollDegrees)
		{
			// Extracts the Euler angles from a rotation matrix. The returned
			// angles are in degrees. This method might suffer from numerical
			// imprecision for ill defined rotation matrices.
			//
			// This function only works for rotation matrices constructed using
			// the popular NASA standard airplane convention of heading-pitch-roll 
			// (i.e., RzRxRy).
			//
			// The algorithm used is from:
			//  David Eberly, "Euler Angle Formulas", Geometric Tools web site,
			//  http://www.geometrictools.com/Documentation/EulerAngles.pdf.

			float thetaX = (float)Math.Sin(mtx[1,2]);
			float thetaY = 0.0f;
			float thetaZ = 0.0f;

			if (thetaX < (Math.PI / 2))
			{
				if (thetaX > -(Math.PI / 2))
				{
					thetaZ = (float)Math.Atan2(-mtx[1,0], mtx[1,1]);
					thetaY = (float)Math.Atan2(-mtx[0,2], mtx[2,2]);
				}
				else
				{
					// Not a unique solution.
					thetaZ = -(float)Math.Atan2(mtx[2,0], mtx[0,0]);
					thetaY = 0.0f;
				}
			}
			else
			{
				// Not a unique solution.
				thetaZ = (float)Math.Atan2(mtx[2,0], mtx[0,0]);
				thetaY = 0.0f;
			}

			headDegrees = VCMath.radiansToDegrees(thetaY);
			pitchDegrees = VCMath.radiansToDegrees(thetaX);
			rollDegrees = VCMath.radiansToDegrees(thetaZ);
		}

		public void toAxes(Vector3 x, Vector3 y, Vector3 z)
		{
			x.Set(mtx[0,0], mtx[0,1], mtx[0,2]);
			y.Set(mtx[1,0], mtx[1,1], mtx[1,2]);
			z.Set(mtx[2,0], mtx[2,1], mtx[2,2]);
		}

		public void toAxesTransposed(Vector3 x, Vector3 y, Vector3 z)
		{
			x.Set(mtx[0,0], mtx[1,0], mtx[2,0]);
			y.Set(mtx[0,1], mtx[1,1], mtx[2,1]);
			z.Set(mtx[0,2], mtx[1,2], mtx[2,2]);
		}

		public Matrix4 transpose()
		{
			Matrix4 tmp = new Matrix4();

			tmp[0,0] = mtx[0,0];tmp[0,1] = mtx[1,0];tmp[0,2] = mtx[2,0];tmp[0,3] = mtx[3,0];
			tmp[1,0] = mtx[0,1];tmp[1,1] = mtx[1,1];tmp[1,2] = mtx[2,1];tmp[1,3] = mtx[3,1];
			tmp[2,0] = mtx[0,2];tmp[2,1] = mtx[1,2];tmp[2,2] = mtx[2,2];tmp[2,3] = mtx[3,2];
			tmp[3, 0] = mtx[0, 3]; tmp[3, 1] = mtx[1, 3]; tmp[3, 2] = mtx[2, 3]; tmp[3, 3] = mtx[3, 3];

			return tmp;
		}

		public float[] toFloatArray()
		{
			float[] flAr = new float[4 * 4];
			flAr[0] = mtx[0, 0];
			flAr[1] = mtx[0, 1];
			flAr[2] = mtx[0, 2];
			flAr[3] = mtx[0, 3];
			flAr[4] = mtx[1, 0];
			flAr[5] = mtx[1, 1];
			flAr[6] = mtx[1, 2];
			flAr[7] = mtx[1, 3];
			flAr[8] = mtx[2, 0];
			flAr[9] = mtx[2, 1];
			flAr[10] = mtx[2, 2];
			flAr[11] = mtx[2, 3];
			flAr[12] = mtx[3, 0];
			flAr[13] = mtx[3, 1];
			flAr[14] = mtx[3, 2];
			flAr[15] = mtx[3, 3];
			return flAr;
		}
	};
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCMDL.NET
{
    public enum ViewType { vtXY, vtXZ, vtZY, vtCamera };
	public enum EActionType
	{
		Select,
		Rotate,
		Move,
		Scale,

		CreateVertex,

		BuildFace,
		BuildingFace1,
		BuildingFace2,

		CreateBone,

		Pan = 256,
		Creating = 512,
		Selecting = 1024,
	};
    public enum CAxisType { atLocal, atView, atWorld };

    public enum SkinVertPos { All, Front, Back, Left, Right, Top, Bottom };
    public enum TModelFileType { ftAQM, ftMDL, ftMD2, ftMD3, ftASC };

    public enum ShadingType { shNone, shFlat, shGourad, shPhong };
    public enum TextureType { ttNone, ttWire, ttAffine, ttPers, ttBilinear };
    public enum NormalDType { ndNone, ndSelected, ndAll };

	public class CModelConstants
	{
		public const int MaxSkinName = 64,
			MaxFrameName = 16;
	};

    public class CSkinVertex
    {
        public float s = 0;						// horiz
        public float t = 0;						// vert
		public ESkinVertexFlags Flags = 0;
    };

    public class CTriangle
    {
		public int[] Vertices = new int[3],
						SkinVerts = new int[3];
		public Vector3	Centre = Vector3.Empty,
						Normal = Vector3.Empty;

		public ETriangleFlags Flags = 0;

		public void Flip()
		{
			int v0, v2;

			v0 = Vertices[0];
			v2 = Vertices[2];

			Vertices[0] = v2;
			Vertices[2] = v0;

			v0 = SkinVerts[0];
			v2 = SkinVerts[2];

			SkinVerts[0] = v2;
			SkinVerts[2] = v0;
		}
	};

	public class CVerticeFrameData
	{
		public Vector3 Position = Vector3.Empty,
						Normal = Vector3.Empty;

		public CVerticeFrameData Copy()
		{
			CVerticeFrameData fd = new CVerticeFrameData();
			fd.Position = Position;
			fd.Normal = Normal;
			return fd;
		}
	};

    public class CVertice
    {
		public List<CVerticeFrameData> FrameData = new List<CVerticeFrameData>();
		public EVerticeFlags Flags = 0;
		public int Bone = -1;
		public Vector3 BoneBasePosition = Vector3.Empty;
		public Vector3 BoneBaseAngles = Vector3.Empty;

		public CVertice Copy()
		{
			CVertice newVert = new CVertice();
			newVert.Flags = Flags;

			for (int i = 0; i < FrameData.Count; ++i)
				newVert.FrameData.Add(FrameData[i].Copy());

			newVert.Bone = Bone;
			newVert.BoneBasePosition = BoneBasePosition;

			return newVert;
		}
	};

	public enum EBoneFlags
	{
		Selected = 1,
		TempSelected = 2,
	};

	public class CBone
	{
		public Vector3 Position = Vector3.Empty,
					   Angles = Vector3.Empty;

		public EBoneFlags Flags = 0;

		public CBone Parent = null;
	};

	public class CMesh
	{
		public List<CSkinVertex> SkinVerts = new List<CSkinVertex>();
		public List<CTriangle> Tris = new List<CTriangle>();
		public List<CVertice> Verts = new List<CVertice>();
		public int SkinIndex;
		public string Name;

		public override string ToString()
		{
			return Name;
		}
	};

    public class line_t
    {
        public float x1, y1, x2, y2, m, c;

        public line_t(float nx1, float ny1, float nx2, float ny2)
        {
            x1 = nx1;
            x2 = nx2;
            y1 = ny1;
            y2 = ny2;
        }

        public bool calcm()
        {
            if (x1 == x2)
                return false;

            m = (y1 - y2) / (x1 - x2);
            return true;
        }

        public bool calcc()
        {
            if (!calcm())
                return false;

            c = y1 - m * x1;

            return true;
        }

        public bool Vertical()
        {
            return (x1 == x2);
        }

        public bool Horionztal()
        {
            return (y1 == y2);
        }
    };

    public class MDLMath
    {
        public static bool InRect(float x1, float y1, float x2, float y2, float a, float b)
        {
            if (x1 > x2)
            {
                float temp = x1;
                x1 = x2;
                x2 = temp;
            }
            if (y1 > y2)
            {
                float temp = y1;
                y1 = y2;
                y2 = temp;
            }

            return (a >= x1 && a <= x2 && b >= y1 && b <= y2);
        }

        public static bool Between(float a, float min, float max)
        {
            if (min > max)
            {
                float temp = max;
                max = min;
                min = temp;
            }

            return (a >= min && a <= max);
        }

        public static bool InTri(float x1, float y1, float x2, float y2, float x3, float y3, float xp, float yp)
        {
            float minx, maxx, miny, maxy;

            miny = maxy = y1;
            minx = maxx = x1;

            if (x2 < minx)
                minx = x2;
            if (y2 < miny)
                miny = y2;

            if (x3 < minx)
                minx = x3;
            if (y3 < miny)
                miny = y3;

            if (x2 > maxx)
                maxx = x2;
            if (y2 > maxy)
                maxy = y2;

            if (x3 > maxx)
                maxx = x3;
            if (y3 > maxy)
                maxy = y3;

            if (xp < minx || yp < miny || xp > maxx || yp > maxy)
                return false;

            float tx1, tx2, tx3, ty1, ty2, ty3;

            tx1 = (x2 + x1) / 2;
            tx2 = (x3 + x2) / 2;
            tx3 = (x1 + x3) / 2;
            ty1 = (y2 + y1) / 2;
            ty2 = (y3 + y2) / 2;
            ty3 = (y1 + y3) / 2;

            line_t a = new line_t(x1, y1, x2, y2),
                        b = new line_t(x2, y2, x3, y3),
                        c = new line_t(x3, y3, x1, y1),
                    p1 = new line_t(xp, yp, tx1, ty1),
                    p2 = new line_t(xp, yp, tx2, ty2),
                    p3 = new line_t(xp, yp, tx3, ty3);

            bool res1, res2, res3;

            res1 = LinesIntersect(a, p2) || LinesIntersect(a, p3);
            res2 = LinesIntersect(b, p1) || LinesIntersect(b, p3);
            res3 = LinesIntersect(c, p1) || LinesIntersect(c, p2);

            return !(res1 || res2 || res3);
        }

        public static bool LinesIntersect(line_t a, line_t b)
        {
            float ix, iy;

            if (a.Vertical() && b.Vertical())
                return false;

            if (a.Vertical()) // first line is vertical
            {
                b.calcm();
                b.calcc();

                ix = a.x1;

                if (!Between(ix, b.x1, b.x2))
                    return false;

                iy = b.m * ix + b.c;
                return (Between(iy, b.y1, b.y2) && Between(iy, a.y1, a.y2));
            }
            if (b.Vertical()) // first line is vertical
            {
                a.calcm();
                a.calcc();

                ix = b.x1;

                if (!Between(ix, a.x1, a.x2))
                    return false;

                iy = a.m * ix + a.c;
                return (Between(iy, a.y1, a.y2) && Between(iy, b.y1, b.y2));
            }

            /*    Refernce:
                y = m1*x + c1;
               y = m2*x + c2;

                m1*x = x2*x + c2 - c1;
               x*(m1-m2) = c2 - c1;
               x = (c2-c1)/(m1-m2)

               (y-c2)/m2 = (y-c1)/m1;
               m1*(y-c2) = m2*(y-c1)
               m1y - m1c2 = m2y - m2c1;
               m1y-m2y = m1c2-m2c1
               y = (m1c2-m2c1)/(m1-m2)
            */

            a.calcm();
            a.calcc();
            b.calcm();
            b.calcc();

            if (a.m == b.m)
                return false;

            ix = (b.c - a.c) / (a.m - b.m);
            iy = a.m * ix + a.c;

            return (Between(ix, a.x1, a.x2) && Between(ix, b.x1, b.x2) &&
                 Between(iy, a.y1, a.y2) && Between(iy, b.y1, b.y2));
        }
    };
}

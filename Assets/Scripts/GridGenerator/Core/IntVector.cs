// Author: Xin Zhang <cowcoa@gmail.com>

using UnityEngine;

namespace Cow
{
    public struct Int3
    {
        public int x;
        public int y;
        public int z;

        public const int Precision = 1000;
        public const float FloatPrecision = 1000F;
        public const float PrecisionFactor = 0.001F;

        private static Int3 _zero = new Int3(0, 0, 0);
        public static Int3 zero { get { return _zero; } }

        public Int3(Vector3 position)
        {
            x = (int)System.Math.Round(position.x * FloatPrecision);
            y = (int)System.Math.Round(position.y * FloatPrecision);
            z = (int)System.Math.Round(position.z * FloatPrecision);
        }


        public Int3(int _x, int _y, int _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        public static bool operator ==(Int3 lhs, Int3 rhs)
        {
            return lhs.x == rhs.x &&
                    lhs.y == rhs.y &&
                    lhs.z == rhs.z;
        }

        public static bool operator !=(Int3 lhs, Int3 rhs)
        {
            return lhs.x != rhs.x ||
                    lhs.y != rhs.y ||
                    lhs.z != rhs.z;
        }

        public static explicit operator Int3(Vector3 ob)
        {
            return new Int3(
                (int)System.Math.Round(ob.x * FloatPrecision),
                (int)System.Math.Round(ob.y * FloatPrecision),
                (int)System.Math.Round(ob.z * FloatPrecision)
                );
        }

        public static explicit operator Vector3(Int3 ob)
        {
            return new Vector3(ob.x * PrecisionFactor, ob.y * PrecisionFactor, ob.z * PrecisionFactor);
        }

        public static Int3 operator -(Int3 lhs, Int3 rhs)
        {
            lhs.x -= rhs.x;
            lhs.y -= rhs.y;
            lhs.z -= rhs.z;
            return lhs;
        }

        public static Int3 operator -(Int3 lhs)
        {
            lhs.x = -lhs.x;
            lhs.y = -lhs.y;
            lhs.z = -lhs.z;
            return lhs;
        }

        public static Int3 operator +(Int3 lhs, Int3 rhs)
        {
            lhs.x += rhs.x;
            lhs.y += rhs.y;
            lhs.z += rhs.z;
            return lhs;
        }

        public static Int3 operator *(Int3 lhs, int rhs)
        {
            lhs.x *= rhs;
            lhs.y *= rhs;
            lhs.z *= rhs;

            return lhs;
        }

        public static Int3 operator *(Int3 lhs, float rhs)
        {
            lhs.x = (int)System.Math.Round(lhs.x * rhs);
            lhs.y = (int)System.Math.Round(lhs.y * rhs);
            lhs.z = (int)System.Math.Round(lhs.z * rhs);

            return lhs;
        }

        public static Int3 operator *(Int3 lhs, double rhs)
        {
            lhs.x = (int)System.Math.Round(lhs.x * rhs);
            lhs.y = (int)System.Math.Round(lhs.y * rhs);
            lhs.z = (int)System.Math.Round(lhs.z * rhs);

            return lhs;
        }

        public static Int3 operator *(Int3 lhs, Vector3 rhs)
        {
            lhs.x = (int)System.Math.Round(lhs.x * rhs.x);
            lhs.y = (int)System.Math.Round(lhs.y * rhs.y);
            lhs.z = (int)System.Math.Round(lhs.z * rhs.z);

            return lhs;
        }

        public static Int3 operator /(Int3 lhs, float rhs)
        {
            lhs.x = (int)System.Math.Round(lhs.x / rhs);
            lhs.y = (int)System.Math.Round(lhs.y / rhs);
            lhs.z = (int)System.Math.Round(lhs.z / rhs);
            return lhs;
        }

        public Int3 DivBy2()
        {
            x >>= 1;
            y >>= 1;
            z >>= 1;
            return this;
        }

        public int this[int i]
        {
            get
            {
                return i == 0 ? x : (i == 1 ? y : z);
            }
            set
            {
                if (i == 0) x = value;
                else if (i == 1) y = value;
                else z = value;
            }
        }

        public static float Angle(Int3 lhs, Int3 rhs)
        {
            double cos = Dot(lhs, rhs) / ((double)lhs.magnitude * (double)rhs.magnitude);
            cos = cos < -1 ? -1 : (cos > 1 ? 1 : cos);
            return (float)System.Math.Acos(cos);
        }

        public static int Dot(Int3 lhs, Int3 rhs)
        {
            return
                    lhs.x * rhs.x +
                    lhs.y * rhs.y +
                    lhs.z * rhs.z;
        }

        public static long DotLong(Int3 lhs, Int3 rhs)
        {
            return
                    (long)lhs.x * (long)rhs.x +
                    (long)lhs.y * (long)rhs.y +
                    (long)lhs.z * (long)rhs.z;
        }

        public Int3 Normal2D()
        {
            return new Int3(z, y, -x);
        }

        public Int3 NormalizeTo(int newMagn)
        {
            float magn = magnitude;

            if (magn == 0)
            {
                return this;
            }

            x *= newMagn;
            y *= newMagn;
            z *= newMagn;

            x = (int)System.Math.Round(x / magn);
            y = (int)System.Math.Round(y / magn);
            z = (int)System.Math.Round(z / magn);

            return this;
        }

        public float magnitude
        {
            get
            {
                double _x = x;
                double _y = y;
                double _z = z;

                return (float)System.Math.Sqrt(_x * _x + _y * _y + _z * _z);
            }
        }

        public int costMagnitude
        {
            get
            {
                return (int)System.Math.Round(magnitude);
            }
        }

        public float worldMagnitude
        {
            get
            {
                double _x = x;
                double _y = y;
                double _z = z;

                return (float)System.Math.Sqrt(_x * _x + _y * _y + _z * _z) * PrecisionFactor;
            }
        }

        public float sqrMagnitude
        {
            get
            {
                double _x = x;
                double _y = y;
                double _z = z;
                return (float)(_x * _x + _y * _y + _z * _z);
            }
        }

        public long sqrMagnitudeLong
        {
            get
            {
                long _x = x;
                long _y = y;
                long _z = z;
                return (_x * _x + _y * _y + _z * _z);
            }
        }

        public int unsafeSqrMagnitude
        {
            get
            {
                return x * x + y * y + z * z;
            }
        }

        public static implicit operator string(Int3 ob)
        {
            return ob.ToString();
        }

        public override string ToString()
        {
            return "( " + x + ", " + y + ", " + z + ")";
        }

        public override bool Equals(System.Object o)
        {

            if (o == null) return false;

            var rhs = (Int3)o;

            return x == rhs.x &&
                    y == rhs.y &&
                    z == rhs.z;
        }

        public override int GetHashCode()
        {
            return x * 73856093 ^ y * 19349663 ^ z * 83492791;
        }
    }

    public struct Int2
    {
        public int x;
        public int y;

        public Int2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int sqrMagnitude
        {
            get
            {
                return x * x + y * y;
            }
        }

        public long sqrMagnitudeLong
        {
            get
            {
                return (long)x * (long)x + (long)y * (long)y;
            }
        }

        public static Int2 operator +(Int2 a, Int2 b)
        {
            return new Int2(a.x + b.x, a.y + b.y);
        }

        public static Int2 operator -(Int2 a, Int2 b)
        {
            return new Int2(a.x - b.x, a.y - b.y);
        }

        public static bool operator ==(Int2 a, Int2 b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Int2 a, Int2 b)
        {
            return a.x != b.x || a.y != b.y;
        }

        public static int Dot(Int2 a, Int2 b)
        {
            return a.x * b.x + a.y * b.y;
        }

        public static long DotLong(Int2 a, Int2 b)
        {
            return (long)a.x * (long)b.x + (long)a.y * (long)b.y;
        }

        public override bool Equals(System.Object o)
        {
            if (o == null) return false;
            var rhs = (Int2)o;

            return x == rhs.x && y == rhs.y;
        }

        public override int GetHashCode()
        {
            return x * 49157 + y * 98317;
        }

        private static readonly int[] Rotations = {
			 1, 0,
			 0, 1,

			 0, 1,
			-1, 0,

			-1, 0,
			 0,-1,

			 0,-1,
			 1, 0
		};

        public static Int2 Rotate(Int2 v, int r)
        {
            r = r % 4;
            return new Int2(v.x * Rotations[r * 4 + 0] + v.y * Rotations[r * 4 + 1], v.x * Rotations[r * 4 + 2] + v.y * Rotations[r * 4 + 3]);
        }

        public static Int2 Min(Int2 a, Int2 b)
        {
            return new Int2(System.Math.Min(a.x, b.x), System.Math.Min(a.y, b.y));
        }

        public static Int2 Max(Int2 a, Int2 b)
        {
            return new Int2(System.Math.Max(a.x, b.x), System.Math.Max(a.y, b.y));
        }

        public static Int2 FromInt3XZ(Int3 o)
        {
            return new Int2(o.x, o.z);
        }

        public static Int3 ToInt3XZ(Int2 o)
        {
            return new Int3(o.x, 0, o.y);
        }

        public override string ToString()
        {
            return "(" + x + ", " + y + ")";
        }
    }
}

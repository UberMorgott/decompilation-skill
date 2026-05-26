using System;
using System.Collections.Generic;

namespace AGInterfaces;

[Serializable]
public struct Quadro<T> : IFormattable, IComparable, IComparable<Quadro<T>>, IEquatable<Quadro<T>> where T : IFormattable, IComparable, IComparable<T>, IEquatable<T>
{
	public static readonly Quadro<T> Empty;

	private static readonly T empty;

	public T q1;

	public T q2;

	public T q3;

	public T q4;

	public bool IsEmpty => Equals(Empty);

	public T[] ToArray()
	{
		ref T reference = ref q1;
		T other = empty;
		if (reference.Equals(other))
		{
			return new T[0];
		}
		ref T reference2 = ref q2;
		T other2 = empty;
		if (!reference2.Equals(other2))
		{
			ref T reference3 = ref q3;
			T other3 = empty;
			if (!reference3.Equals(other3))
			{
				ref T reference4 = ref q4;
				T other4 = empty;
				if (!reference4.Equals(other4))
				{
					return new T[4] { q1, q2, q3, q4 };
				}
				return new T[3] { q1, q2, q3 };
			}
			return new T[2] { q1, q2 };
		}
		return new T[1] { q1 };
	}

	public Quadro(T[] qs)
	{
		if (qs == null || qs.Length != 4)
		{
			throw new ArgumentException("qs is not 4 bytes long.");
		}
		q1 = qs[0];
		q2 = qs[1];
		q3 = qs[2];
		q4 = qs[3];
	}

	public Quadro(T q1)
	{
		this.q1 = q1;
		q2 = (q3 = (q4 = empty));
	}

	public Quadro(T q1, T q2, T q3, T q4)
	{
		this.q1 = q1;
		this.q2 = q2;
		this.q3 = q3;
		this.q4 = q4;
	}

	public static bool operator !=(Quadro<T> a, Quadro<T> b)
	{
		return !a.Equals(b);
	}

	public static bool operator ==(Quadro<T> a, Quadro<T> b)
	{
		return a.Equals(b);
	}

	public static implicit operator T(Quadro<T> a)
	{
		return a.q1;
	}

	public static implicit operator Quadro<T>(T q1)
	{
		return new Quadro<T>(q1);
	}

	public int CompareTo(object obj)
	{
		if (!(obj is Quadro<T>))
		{
			return -1;
		}
		if (obj == null)
		{
			return 1;
		}
		return CompareTo((Quadro<T>)obj);
	}

	public int CompareTo(Quadro<T> other)
	{
		ref T reference = ref q4;
		T other2 = other.q4;
		int num = reference.CompareTo(other2);
		if (num != 0)
		{
			return num;
		}
		ref T reference2 = ref q3;
		T other3 = other.q3;
		num = reference2.CompareTo(other3);
		if (num != 0)
		{
			return num;
		}
		ref T reference3 = ref q2;
		T other4 = other.q2;
		num = reference3.CompareTo(other4);
		if (num != 0)
		{
			return num;
		}
		ref T reference4 = ref q1;
		T other5 = other.q1;
		return reference4.CompareTo(other5);
	}

	public int CompareTo(T q1)
	{
		ref T reference = ref q4;
		T other = empty;
		int num = reference.CompareTo(other);
		if (num != 0)
		{
			return num;
		}
		ref T reference2 = ref q3;
		T other2 = empty;
		num = reference2.CompareTo(other2);
		if (num != 0)
		{
			return num;
		}
		ref T reference3 = ref q2;
		T other3 = empty;
		num = reference3.CompareTo(other3);
		if (num != 0)
		{
			return num;
		}
		return q1.CompareTo(q1);
	}

	public bool Equals(Quadro<T> other)
	{
		ref T reference = ref q1;
		T other2 = other.q1;
		if (reference.Equals(other2))
		{
			ref T reference2 = ref q2;
			T other3 = other.q2;
			if (reference2.Equals(other3))
			{
				ref T reference3 = ref q3;
				T other4 = other.q3;
				if (reference3.Equals(other4))
				{
					ref T reference4 = ref q4;
					T other5 = other.q4;
					return reference4.Equals(other5);
				}
			}
		}
		return false;
	}

	public bool Equals(T q1)
	{
		if (q1.Equals(q1))
		{
			ref T reference = ref q2;
			T other = empty;
			if (reference.Equals(other))
			{
				ref T reference2 = ref q3;
				T other2 = empty;
				if (reference2.Equals(other2))
				{
					ref T reference3 = ref q4;
					T other3 = empty;
					return reference3.Equals(other3);
				}
			}
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is Quadro<T>))
		{
			return false;
		}
		return Equals((Quadro<T>)obj);
	}

	public bool Contains(T q)
	{
		if (!q1.Equals(q) && !q2.Equals(q) && !q3.Equals(q))
		{
			return q4.Equals(q);
		}
		return true;
	}

	public bool ContainsAll(Quadro<T> other)
	{
		ref T reference = ref other.q1;
		T other2 = empty;
		if (reference.Equals(other2))
		{
			return false;
		}
		if (!Contains(other.q1))
		{
			return false;
		}
		ref T reference2 = ref other.q2;
		T other3 = empty;
		if (reference2.Equals(other3))
		{
			return true;
		}
		if (!Contains(other.q2))
		{
			return false;
		}
		ref T reference3 = ref other.q3;
		T other4 = empty;
		if (reference3.Equals(other4))
		{
			return true;
		}
		if (!Contains(other.q3))
		{
			return false;
		}
		ref T reference4 = ref other.q4;
		T other5 = empty;
		if (reference4.Equals(other5))
		{
			return true;
		}
		if (!Contains(other.q4))
		{
			return false;
		}
		return true;
	}

	public bool Add(T q)
	{
		ref T reference = ref q1;
		T other = empty;
		if (reference.Equals(other))
		{
			q1 = q;
			return true;
		}
		if (!q1.Equals(q))
		{
			ref T reference2 = ref q2;
			T other2 = empty;
			if (reference2.Equals(other2))
			{
				q2 = q;
				return true;
			}
			if (!q2.Equals(q))
			{
				ref T reference3 = ref q3;
				T other3 = empty;
				if (reference3.Equals(other3))
				{
					q3 = q;
					return true;
				}
				if (!q3.Equals(q))
				{
					ref T reference4 = ref q4;
					T other4 = empty;
					if (reference4.Equals(other4))
					{
						q4 = q;
						return true;
					}
				}
			}
		}
		return false;
	}

	public bool Remove(T q)
	{
		if (q1.Equals(q))
		{
			q1 = q2;
			q2 = q3;
			q3 = q4;
			q4 = empty;
			return true;
		}
		if (q2.Equals(q))
		{
			q2 = q3;
			q3 = q4;
			q4 = empty;
			return true;
		}
		if (q3.Equals(q))
		{
			q3 = q4;
			q4 = empty;
			return true;
		}
		if (q4.Equals(q))
		{
			q4 = empty;
			return true;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return q1.GetHashCode() ^ q2.GetHashCode() ^ q3.GetHashCode() ^ q4.GetHashCode();
	}

	public override string ToString()
	{
		string text = null;
		ref T reference = ref q1;
		T other = empty;
		if (!reference.Equals(other))
		{
			text = q1.ToString();
			ref T reference2 = ref q2;
			T other2 = empty;
			if (!reference2.Equals(other2))
			{
				text = ((text == null) ? q2.ToString() : (text + ", " + q2.ToString()));
				ref T reference3 = ref q3;
				T other3 = empty;
				if (!reference3.Equals(other3))
				{
					text = ((text == null) ? q3.ToString() : (text + ", " + q3.ToString()));
					ref T reference4 = ref q4;
					T other4 = empty;
					if (!reference4.Equals(other4))
					{
						text = ((text == null) ? q4.ToString() : (text + ", " + q4.ToString()));
					}
				}
			}
		}
		return text;
	}

	public string ToString(string format, IFormatProvider formatProvider)
	{
		string text = null;
		ref T reference = ref q1;
		T other = empty;
		if (!reference.Equals(other))
		{
			text = q1.ToString();
			ref T reference2 = ref q2;
			T other2 = empty;
			if (!reference2.Equals(other2))
			{
				text = ((text == null) ? q2.ToString(format, formatProvider) : (text + ", " + q2.ToString(format, formatProvider)));
				ref T reference3 = ref q3;
				T other3 = empty;
				if (!reference3.Equals(other3))
				{
					text = ((text == null) ? q3.ToString(format, formatProvider) : (text + ", " + q3.ToString(format, formatProvider)));
					ref T reference4 = ref q4;
					T other4 = empty;
					if (!reference4.Equals(other4))
					{
						text = ((text == null) ? q4.ToString(format, formatProvider) : (text + ", " + q4.ToString(format, formatProvider)));
					}
				}
			}
		}
		return text;
	}

	public IEnumerable<T> Enumerate()
	{
		ref T reference = ref q1;
		T other = empty;
		if (reference.Equals(other))
		{
			yield break;
		}
		yield return q1;
		ref T reference2 = ref q2;
		T other2 = empty;
		if (reference2.Equals(other2))
		{
			yield break;
		}
		yield return q2;
		ref T reference3 = ref q3;
		T other3 = empty;
		if (!reference3.Equals(other3))
		{
			yield return q3;
			ref T reference4 = ref q4;
			T other4 = empty;
			if (!reference4.Equals(other4))
			{
				yield return q4;
			}
		}
	}
}

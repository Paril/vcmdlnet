using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace VCMDL.NET
{
	public static class Extensions
	{
		public static void Add<T1, T2>(this Dictionary<T1, T2> dic, KeyValuePair<T1, T2> pair)
		{
			dic.Add(pair.Key, pair.Value);
		}

		public static List<T> Invert<T>(this List<T> lst)
		{
			List<T> newList = new List<T>();

			for (int i = lst.Count - 1; i >= 0; --i)
				newList.Add(lst[i]);

			return newList;
		}

		public class ReverseComparer<T> : IComparer<T>
		{
			public int Compare(T x, T y)
			{
				if ((int)(object)x == (int)(object)y)
					return 0;
				else
					return ((int)(object)x > (int)(object)y) ? -1 : 1;
			}
		}

		public static void ReverseSort<T>(this List<T> lst)
		{
			lst.Sort(new ReverseComparer<T>());
		}

		public static void Insert<T>(this List<T> lst, int Index, List<T> lst2)
		{
			for (int i = lst2.Count - 1; i >= 0; i--)
				lst.Insert(Index, lst2[i]);
		}
	}

	public class IniFile
	{
		Dictionary<string, Dictionary<string, string>> Entries = new Dictionary<string,Dictionary<string,string>>();

		public Dictionary<string, string> GetSection(string section)
		{
			if (!Entries.ContainsKey(section))
				return null;
			else
				return Entries[section];
		}

		public bool Valid()
		{
			return Entries.Count != 0;
		}

		public void AddSection(string section, Dictionary<string, string> values)
		{
			if (Entries.ContainsKey(section))
			{
				// merge
				foreach (KeyValuePair<string, string> pair in values)
				{
					if (Entries[section].ContainsKey(pair.Key))
						Entries[section][pair.Key] = pair.Value;
					else
						Entries[section].Add(pair);
				}

			}
			else
				Entries.Add(section, values);
		}

		public void RemoveSection(string section)
		{
			if (Entries.ContainsKey(section))
				Entries.Remove(section);
		}

		public void SaveIni(string fileName)
		{
			using (FileStream fs = File.Open(fileName, FileMode.Create))
			{
				using (StreamWriter sr = new StreamWriter(fs))
				{
					sr.WriteLine("; Saved by VCMDL.NET");
					sr.WriteLine();

					foreach (KeyValuePair<string, Dictionary<string, string>> f in Entries)
					{
						if (f.Key != "")
							sr.WriteLine ("[" + f.Key + "]");

						foreach (KeyValuePair<string, string> d in f.Value)
							sr.WriteLine(d.Key+"="+d.Value);

						sr.WriteLine();
					}
				}
			}
		}	

		public IniFile(string fileName)
		{
			if (File.Exists(fileName))
			{
				using (FileStream fs = File.Open(fileName, FileMode.Open))
				{
					using (StreamReader sr = new StreamReader(fs))
					{
						string currentSection = "";

						while (sr.EndOfStream == false)
						{
							string line = sr.ReadLine();

							if (string.IsNullOrEmpty(line))
								continue;
							else if (line.StartsWith("//") || line.StartsWith("#") || line.StartsWith(";"))
								continue;

							if (line[0] == '[')
							{
								currentSection = line.Substring(1, line.Length - 2);
								Entries.Add(currentSection, new Dictionary<string, string>());
							}
							else
							{
								string key = line.Substring(0, line.IndexOf('='));
								string value = line.Substring(line.IndexOf('=')+1);

								key = key.Trim();
								value = value.Trim();

								if (Entries.ContainsKey(currentSection) == false)
									Entries.Add(currentSection, new Dictionary<string, string>());

								value = value.Replace("\\n", "\n").
								Replace("\\;", ";").
								Replace("\\#", "#").
								Replace("\\\\", "\\").
								Replace("\\t", "\t").
								Replace("\\r", "\r");

								if (Entries[currentSection].ContainsKey(key) == false)
									Entries[currentSection].Add(key, value);
								else
									Entries[currentSection][key] = value;
							}
						}
					}
				}
			}
		}
	}
}

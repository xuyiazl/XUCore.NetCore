using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.Base.Utils.Files
{
    enum _CharType
    {
        Number = 0,
        Normal = 1
    }

    class _FileModel
    {
        private string _oValue;
        public string oValue
        {
            set { _oValue = value; }
            get { return _oValue; }
        }

        private _CharType _cType;
        public _CharType cType
        {
            set { _cType = value; }
            get { return _cType; }
        }

        public _FileModel(string o, _CharType c)
        {
            _oValue = o;
            _cType = c;
        }
    }

    public class FileNameComparer : IComparer
    {
        public int Compare(object a, object b)
        {
            string s1 = a.ToString();
            string s2 = b.ToString();

            if (s1.IndexOf('.') == -1 && s2.IndexOf('.') == -1)
            {
                return SubCompare(s1, s2);
            }

            else if (s1.IndexOf('.') != -1 && s2.IndexOf('.') != -1)
            {
                int pos1 = s1.IndexOf(".");
                int pos2 = s2.IndexOf(".");
                string ss1 = s1.Substring(0, pos1);
                string ss2 = s2.Substring(0, pos2);
                int result = SubCompare(ss1, ss2);
                if (result == 0)
                {
                    string is1 = s1.Substring(pos1 + 1);
                    string is2 = s2.Substring(pos2 + 1);
                    return SubCompare(is1, is2);
                }
                return result;
            }
            else
            {
                int pos1 = s1.IndexOf(".");
                int pos2 = s2.IndexOf(".");
                return pos1 > pos2 ? 1 : -1;
            }
        }

        int SubCompare(string s1, string s2)
        {
            List<_FileModel> q1 = null;
            List<_FileModel> q2 = null;

            q1 = StoreQueue(s1);
            q2 = StoreQueue(s2);
            if (q1 == null)
                return -1;
            if (q2 == null)
                return 1;

            int len = q1.Count;
            if (len > q2.Count)
                len = q2.Count;
            for (int i = 0; i < len; i++)
            {
                if (q1[i].cType != q2[i].cType)
                {
                    return q1[i].oValue[0] > q2[i].oValue[0] ? 1 : -1;
                }
                else
                {
                    if (q1[i].oValue == q2[i].oValue)
                    {
                        continue;
                    }
                    if (q1[i].cType == _CharType.Number)
                    {
                        if (q1[i].oValue != q2[i].oValue)
                        {
                            int num1 = int.Parse(q1[i].oValue);
                            int num2 = int.Parse(q2[i].oValue);
                            return num1 > num2 ? 1 : -1;
                        }
                    }
                    else
                    {
                        if (q1[i].oValue != q2[i].oValue)
                        {
                            int rlen = q1[i].oValue.Length;
                            if (rlen > q2[i].oValue.Length)
                                rlen = q2[i].oValue.Length;
                            for (int j = 0; j < rlen; j++)
                            {
                                if (q1[i].oValue[j] != q2[i].oValue[j])
                                    return q1[i].oValue[j] > q2[i].oValue[j] ? 1 : -1;
                            }
                            return q1[i].oValue.Length > q2[i].oValue.Length ? 1 : -1;
                        }
                    }
                }
            }
            if (q1.Count != q2.Count)
                return q1.Count > q2.Count ? 1 : -1;
            else
                return 0;
        }

        _CharType GetCharType(char c)
        {
            if (c >= 48 && c <= 57)
                return _CharType.Number;
            else return _CharType.Normal;
        }

        List<_FileModel> StoreQueue(string str)
        {

            if (string.IsNullOrEmpty(str) || str.Length == 0)
            {
                return null;
            }

            List<_FileModel> sl = new List<_FileModel>();
            _FileModel m = null;
            _CharType ctype = GetCharType(str[0]);
            if (str.Length == 1)
            {
                m = new _FileModel(str, ctype);
                sl.Add(m);
                return sl;
            }
            int start = 0;
            for (int i = 1; i < str.Length; i++)
            {
                if (GetCharType(str[i]) != ctype)
                {
                    m = new _FileModel(str.Substring(start, i - start), ctype);
                    sl.Add(m);
                    if (i == str.Length - 1)
                    {
                        _CharType sType = GetCharType(str[i]);
                        m = new _FileModel(str[i].ToString(), sType);
                        sl.Add(m);
                    }
                    else
                    {
                        ctype = GetCharType(str[i]);
                        start = i;
                    }
                }
                else
                {
                    if (i == str.Length - 1)
                    {
                        _CharType sType = GetCharType(str[i]);
                        m = new _FileModel(str.Substring(start, i + 1 - start), sType);
                        sl.Add(m);
                    }
                }
            }
            return sl;
        }
    }
}

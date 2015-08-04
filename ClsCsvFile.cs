using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace TPXeroxDocuWorks.Common
{
    public class ClsCsvFile
    {

        private enum ScanState { Normal, Enter };

        public static object[] Read(string filename, string codeName)
        {

            ArrayList lines = new ArrayList();
            string s;
            string[] p;

            if (codeName == "")
            {

                codeName = "Shift_JIS";

            }

            using (StreamReader sr = new StreamReader(filename, Encoding.GetEncoding(codeName)))
            {
                while ((s = sr.ReadLine()) != null)
                {

                    p = GetTokens(s);
                    lines.Add(p);

                }

                sr.Close();

            }
            
            object[] retVal = lines.ToArray();
            return retVal;

        }

        public static string[] GetTokens( string s )
        {

            ArrayList arr = new ArrayList();
            string buff = "";
            ScanState state1 = ScanState.Normal;

            string[] p1 = Regex.Split( s, ",");

            for ( int i = 0; i < p1.Length; i++)
            {

                string ps = p1[ i ];

                switch ( state1 )
                {

                    case ScanState.Normal:
                        if ( ps.IndexOf( "\"" ) == 0 )
                        {

                            if ( ps.IndexOf( "\"", 1 ) == ps.Length - 1 )
                            {

                                arr.Add( ps.Substring( 1, ps.Length - 2 ) );

                            }
                            else
                            {

                                buff = ps.Substring( 1, ps.Length - 1 );

                                state1 = ScanState.Enter;

                            }
                            break;

                        }
                        else
                        {

                            arr.Add( ps );

                        }
                        break;

                    case ScanState.Enter:

                        int nl = ps.IndexOf( "\"" );

                        if ( nl == ps.Length - 1 )
                        {

                            buff += ps.Substring( 0, ps.Length - 1 );

                            arr.Add( buff.Clone() );

                            buff = "";

                            state1 = ScanState.Normal;

                        }
                        else
                        {

                            buff += ps;

                        }
                        break;

                    default:
                        break;

                }

            }

            return ( string[] ) arr.ToArray( Type.GetType( "System String" ) );

        }

        public static void Write(string filename, object[] data, string codeName)
        {

            string s;

            if (codeName == "")
            {

                codeName = "Shift_JIS";

            }

            using (StreamWriter sw = new StreamWriter(filename, false, Encoding.GetEncoding(codeName)))
            {

                for (int i = 0; i < data.Length; i++)
                {

                    s = JoinData((object[])data[i]);

                    sw.WriteLine(s);

                }

                sw.Close();

            }

        }

        public static string JoinData( object[] d )
        {

            string s;
            string[] buff = new string[ d.Length ];

            for ( int i = 0; i < d.Length; i++ )
            {

                s = d[ i ].ToString();

                if ( s.IndexOf( "," ) >= 0 )
                {

                    s = "\"" + s + "\"";

                }

                buff[ i ] = s;

            }

            string retVal = String.Join( ",", buff );

            return retVal;

        }
        
    }

}


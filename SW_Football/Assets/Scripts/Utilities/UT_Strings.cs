/*************************************************************************************
Helper class
*************************************************************************************/
using UnityEngine;

public static class UT_Strings
{
    // returns the first substring that starts with first char, and ends with last char.
    // eg. "Help(0,5)(10,5)" Would return (0,5) if we passed in (s, '(',')')
    public static string StartAndEndString(string s, char cStart, char cEnd)
    {
        int startPos = -1;
        for(int i=0; i<s.Length; i++)
        {
            if(s[i] == cStart){
                startPos = i;
                break;
            }
        }
        if(startPos == -1){
            return "START CHAR NOT FOUND\n";
        }

        bool foundEnd = false;
        int endPos = startPos+1;
        for(int i=endPos; i<s.Length; i++){
            if(s[i] == cEnd){
                endPos = i;
                foundEnd = true;
                break;
            }
        }
        if(!foundEnd){
            return "END CHAR NOT FOUND\n";
        }

        return s.Substring(startPos, (endPos-startPos + 1));
    }

    public static string StartAndEndString(string s, char cEnd, bool inclusive = false)
    {
        int charPos = -1;
        for(int i=0; i<s.Length; i++){
            if(s[i] == cEnd){
                charPos = i;
                break;
            }
        }
        if(charPos == -1){
            return "CHAR NOT FOUND\n";
        }

        if(!inclusive){
            return s.Substring(0, charPos);
        }
        return s.Substring(0, charPos+1);
    }

    public static string DeleteMultipleChars(string s, string c)
    {
        string copy = s.Substring(0);
        for(int i=0; i<c.Length; i++)
        {
            copy = copy.Replace(c.Substring(i, 1), "");
        }

        return copy;
    }

    // Work in progress, not called anywhere yet.
    public static string ReplaceMultiple(string s, string c, string replacer)
    {
        for(int i=0; i<c.Length; i++){
            s.Replace(c[i], replacer[0]);
        }

        return s;
    }

    public static Vector2 FGetVecFromString(string sVec)
    {
        int commaLoc = sVec.IndexOf(',');
        string xVal = sVec.Substring(1, commaLoc-1);
        int endBrack = sVec.IndexOf(')');
        string yVal = sVec.Substring(commaLoc+1, endBrack - commaLoc - 1);

        return new Vector2(float.Parse(xVal), float.Parse(yVal));
    }

    public static string FConvertVecToString(Vector2 vec2)
    {
        return "("+vec2.x+","+vec2.y+")";
    }
}

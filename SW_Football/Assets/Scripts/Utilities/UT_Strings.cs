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
}

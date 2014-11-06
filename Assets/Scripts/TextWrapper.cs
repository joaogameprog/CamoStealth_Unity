using System.Collections;
using UnityEngine;

/**
 
 TextSize for Unity3D by thienhaflash (thienhaflash@gmail.com)
 
 Version	: 0.1
 Update		: 18.Jun.2012
 Features	:
    Return perfect size for any TextMesh
    Cache the size of each character to speed up the size
    Evaluate and cache only when there are requirements
 
 Sample 	:
		
        //declare it locally, so we can have access anywhere from the script
        TextSize ts;
		
        //put this on the Start function
        ts = new TextSize(gameObject.GetComponent<TextMesh>());
		
        //anywhere, after you change the text :
        print(ts.width);
		
        //or get the length of an abitrary text (that is not assign to TextMesh)
        print(ts.GetTextWidth("any abitrary text goes here"));

 You are free to use the code or modify it any way you want (even remove this block of comments) but if it's good
 please give it back to the community.
 
 */

public class TextWrapper : MonoBehaviour {
	private Hashtable dict; //map character -> width
	
	private TextMesh textMesh;
	new private Renderer renderer;
	new BoxCollider2D collider;
	string lastText = "";
	public void Start ()
	{
		textMesh = GetComponent<TextMesh> ();
		renderer = textMesh.renderer;
		collider = GetComponent<BoxCollider2D> ();
		dict = new Hashtable ();
		getSpace ();

	}
	/*
	public TextSize(TextMesh tm){
		textMesh = tm;
		renderer = tm.renderer;
		dict = new Hashtable();
		getSpace();
	}
	*/

	void MeasureText(string text)
	{
		int index = 0;
		int lineStartIndex = 0;
		int lastSpaceIndex = -1;
		text = text.Replace ("\n", "");
		while (index < text.Length) 
		{
			if(char.IsWhiteSpace(text[index] ))
			{
				if(GetTextWidth(text.Substring(lineStartIndex, index - lineStartIndex)) > collider.bounds.size.x)
				{
					text = text.Insert(lastSpaceIndex != -1 ? lastSpaceIndex : index, "\n");
					textMesh.text = text.Substring(0, lastSpaceIndex);
					if(height > collider.bounds.size.y)
						return;

					lineStartIndex = lastSpaceIndex;
				}
				lastSpaceIndex = index;
			}
			index ++;
		}
		if(GetTextWidth(text.Substring(lineStartIndex, index - lineStartIndex)) > collider.bounds.size.x)
		{
			text = text.Insert(lastSpaceIndex != -1 ? lastSpaceIndex : index, "\n");
			textMesh.text = text.Substring(0, lastSpaceIndex);
			if(height > collider.bounds.size.y)
				return;
			lineStartIndex = lastSpaceIndex;
		}
		textMesh.text = lastText = text;
	}


	void Update()
	{
		if (textMesh.text != lastText)
			MeasureText (textMesh.text);

	}

	private void getSpace(){//the space can not be got alone
		string oldText = textMesh.text;
		
		textMesh.text = "a";
		float aw = renderer.bounds.size.x;
		textMesh.text = "a a";
		float cw = renderer.bounds.size.x - 2* aw;

		dict.Add(' ', cw);
		dict.Add('a', aw);
		
		textMesh.text = oldText;
	}
	
	public float GetTextWidth(string s) {
		char[] charList = s.ToCharArray();
		float w = 0;
		char c;
		string oldText = textMesh.text;
		
		for (int i=0; i<charList.Length; i++){
			c = charList[i];
			
			if (dict.ContainsKey(c)){
				w += (float)dict[c];
			} else {
				textMesh.text = ""+c;
				float cw = renderer.bounds.size.x;
				dict.Add(c, cw);
				w += cw;
				//MonoBehaviour.print("char<" + c +"> " + cw);
			}
		}
		
		textMesh.text = oldText;
		return w;
	}
	
	public float width { get { return GetTextWidth(textMesh.text); } }
	public float height { get { return renderer.bounds.size.y; } }
}
using UnityEngine;
using System.Collections;
using TouchScript.Gestures;

[RequireComponent (typeof(BoxCollider2D))]
[RequireComponent (typeof(TapGesture))]
[RequireComponent (typeof(TextMesh))]
public class EditableText : MonoBehaviour {
	#region Fields
	[SerializeField]
	int id;

	static int focusedId;
	public static int FocusedId{
		get
		{
			return focusedId;
		}
		set
		{
			focusedId = value;
			//if(value != id)
			{
				//.isSelected = false;
				//this.SetInfo(this.isSelected);
			}
		}

	}
	[SerializeField]
	EditableText nextText;

	[SerializeField]
	Color normalColor = Color.white;
	[SerializeField]
	Color selectedColor = Color.blue;

	TapGesture tapGesture;

	TextMesh textMesh;

	BoxCollider2D boxCollider;

	[SerializeField]
	string text;

	public string Text
	{ 
		get
		{
			return text;
		}
		set
		{
			text = value;
			textMesh.text = value;
		}
	}

	[SerializeField]
	string defaultText;

	bool showIndexer;
	float indexerTime = 0.5f;
	float indexerTimer = 0f;
	int indexerPos = 0;
	string shownString = "";
	int shownStartIndex;

	bool isSelected;
	#endregion

	#region Behaviours
	// Use this for initialization
	void Awake () {
		tapGesture = GetComponent<TapGesture> ();
		textMesh = GetComponent<TextMesh> ();
		boxCollider = GetComponent<BoxCollider2D> ();
		if(string.IsNullOrEmpty(defaultText))
		{
			defaultText = textMesh.text;
		}
	}
	
	// Update is called once per frame
	void OnGUI () {
		if(isSelected)
		{
			Event e = Event.current;
			if(e != null && e.type == EventType.KeyDown){
					if(e.keyCode != KeyCode.None){
						if(e.keyCode == KeyCode.Backspace && indexerPos > 0){
							Text = Text.Remove(indexerPos - 1, 1);
							indexerPos = Mathf.Max(0,indexerPos--);
						}else if(e.keyCode == KeyCode.Delete && indexerPos < Text.Length - 1){
							Text = Text.Remove(indexerPos, 1);
							indexerPos = Mathf.Max(0,indexerPos--);
						} 
							else if(e.keyCode == KeyCode.Return)
						{
							Selected(this, System.EventArgs.Empty);
						}
						else if(e.keyCode == KeyCode.Tab)
						{
							Selected(this, System.EventArgs.Empty);
							if(nextText != null)
							{
								nextText.Selected(this, System.EventArgs.Empty);
							}
						}
						else if(e.keyCode == KeyCode.LeftArrow)
						{
							indexerPos --;
						}
						else if(e.keyCode == KeyCode.RightArrow)
						{
							indexerPos ++;
						}
						e.Use();
					}

					else if(e.character != '\t')
					{
						Text = Text.Insert(indexerPos, e.character.ToString());
						indexerPos++;
					}
					
					indexerPos = (int)Mathf.Clamp(indexerPos, 0, Text.Length);
					textMesh.text = Text;
					if(textMesh.renderer.bounds.size.x > boxCollider.bounds.size.x)
					{
						var width = 0;
						var i = 0;
						int removedFromStart = 0;
						int removedFromEnd = 0;
						string newText = Text;
						while(GetTextWidth(newText) > boxCollider.bounds.size.x)
						{
							if(indexerPos > newText.Length / 2){
								newText = newText.Remove(0, 1);
								removedFromStart++;
							}
							else{
								newText = newText.Remove(newText.Length - 1, 1);
								removedFromEnd ++;
							}
						}
						textMesh.text = newText;
						shownStartIndex = removedFromStart;
						
					}else{
						shownStartIndex = 0;
					}
					shownString = textMesh.text;
					/*
					if(Event.current != null){
						if(Event.current.isKey){
							Text = Event.current.keyCode.ToString();
						}
					}
					*/

			}
		}
	}

	void Update()
	{
		if(isSelected)
		{
			if(Input.GetMouseButtonDown(0))
			{
				var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				pos.z = 0;
				if(!boxCollider.bounds.Contains(pos)){
					Selected(this, System.EventArgs.Empty);
				}
			}
			indexerTimer -= Time.deltaTime;

			if(indexerTimer < 0)
			{
				showIndexer = !showIndexer;
				indexerTimer = indexerTime;
				textMesh.text = Text.Insert(indexerPos,(showIndexer ? "|" : ""))
					.Substring(shownStartIndex, shownString.Length);
			}
		}
	}

	void OnEnable()
	{
		tapGesture.Tapped += Selected;

	}

	void OnDisable()
	{
		tapGesture.Tapped -= Selected;
		
	}




	#endregion

	#region Events
	public void Selected (object sender, System.EventArgs e)
	{
		isSelected = !isSelected;
		SetInfo (isSelected);
	}


	void SetInfo (bool selected)
	{
		if (selected) {
			textMesh.color = selectedColor;
			indexerPos = Text.Length;
		}
		else {
			textMesh.color = normalColor;
			string newText = Text;
			while(GetTextWidth(newText) > boxCollider.bounds.size.x)
			{
				newText = newText.Remove(newText.Length - 1, 1);
			}
			textMesh.text = newText;
			if (string.IsNullOrEmpty (textMesh.text)) {
				textMesh.text = defaultText;
			}
		}
	}
	#endregion


	#region TextWrapper
	private Hashtable dict; //map character -> width

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

	#endregion

}

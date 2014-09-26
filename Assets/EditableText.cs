using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using System;
using System.Linq;

[RequireComponent (typeof(BoxCollider2D))]
[RequireComponent (typeof(TapGesture))]
[RequireComponent (typeof(TextMesh))]
public class EditableText : MonoBehaviour {
	#region Fields

	/// <summary>
	/// The action is activated on enter pressed.
	/// </summary>
	public Action OnEnterPressed;

	/// <summary>
	/// The identifier.
	/// </summary>
	[SerializeField]
	int id;

	/// <summary>
	/// The focused identifier.
	/// </summary>
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

	/// <summary>
	/// The next text.
	/// </summary>
	[SerializeField]
	EditableText nextText;

	/// <summary>
	/// The color of the normal state.
	/// </summary>
	[SerializeField]
	Color normalColor = Color.white;

	/// <summary>
	/// The color of the selected state.
	/// </summary>
	[SerializeField]
	Color selectedColor = Color.blue;

	/// <summary>
	/// The tap gesture.
	/// </summary>
	TapGesture tapGesture;

	/// <summary>
	/// The text mesh.
	/// </summary>
	TextMesh textMesh;

	/// <summary>
	/// The box collider.
	/// </summary>
	BoxCollider2D boxCollider;

	/// <summary>
	/// The text.
	/// </summary>
	[SerializeField]
	string text;
	/// <summary>
	/// Gets or sets the text and the textMesh.
	/// </summary>
	/// <value>The text.</value>
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

	/// <summary>
	/// The text showed when the string is empty, 
	/// by default is loaded with textMesh inspector text value.
	/// </summary>
	[SerializeField]
	string defaultText;

	#region Indexer
	/// <summary>
	/// Determines if the indexer character '|' will appear on the textMesh
	/// </summary>
	bool showIndexer;
	/// <summary>
	/// The indexer (in)visible change time.
	/// </summary>
	float indexerTime = 0.5f;

	/// <summary>
	/// The indexer timer.
	/// </summary>
	float indexerTimer = 0f;

	/// <summary>
	/// The indexer position.
	/// </summary>
	int indexerPos = 0;

	#endregion

	/// <summary>
	/// The shown string.
	/// </summary>
	string shownString = "";

	/// <summary>
	/// The index of the shown text start.
	/// </summary>
	int shownStartIndex;

	/// <summary>
	/// Determines if the field is selected.
	/// </summary>
	bool isSelected;

	/// <summary>
	/// The mobile keyboard.
	/// </summary>
	TouchScreenKeyboard mobileKeyboard;

	/// <summary>
	/// The type filter.
	/// </summary>
	[SerializeField]
	TouchScreenKeyboardType type;

	/// <summary>
	/// The field is a password?
	/// </summary>
	[SerializeField]
	bool isPassword;

	/// <summary>
	/// Gets the width of the textMesh.
	/// </summary>
	/// <value>The width.</value>
	public float width { get { return GetTextWidth(textMesh.text); } }

	/// <summary>
	/// Gets the height of the textMesh.
	/// </summary>
	/// <value>The height.</value>
	public float height { get { return renderer.bounds.size.y; } }
	
	#endregion

	#region Behaviours
	/// <summary>
	/// Components initializations.
	/// </summary>
	void Awake () {
		tapGesture = GetComponent<TapGesture> ();
		textMesh = GetComponent<TextMesh> ();
		boxCollider = GetComponent<BoxCollider2D> ();
		if(string.IsNullOrEmpty(defaultText))
		{
			defaultText = textMesh.text;
		}
	}
	
	/// <summary>
	/// Verify if a key is pressed and validate on text.
	/// </summary>
	void OnGUI () {
		if(isSelected)
		{
			Event e = Event.current;
			if(e != null && e.type == EventType.KeyDown || (mobileKeyboard != null)){
					if(e.keyCode != KeyCode.None){
						if(e.keyCode == KeyCode.Backspace && indexerPos > 0){
							Text = Text.Remove(indexerPos - 1, 1);
							indexerPos = Mathf.Max(0,indexerPos--);
						}else if(e.keyCode == KeyCode.Delete && indexerPos < Text.Length){
							Text = Text.Remove(indexerPos, 1);
							indexerPos = Mathf.Max(0,indexerPos--);
						} 
						else if(e.keyCode == KeyCode.Return)
						{
							EnterPressed ();
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
					else if((int)e.character > 31)
					{
						Text = Text.Insert(indexerPos, e.character.ToString());
						indexerPos++;
					}

					if(mobileKeyboard != null)
					{
						if(mobileKeyboard.done)
						{
							isSelected = false;
							SetInfo(isSelected);
						}
						if(mobileKeyboard.active)
						{
							Text = mobileKeyboard.text;
						}
					}
				         
					indexerPos = (int)Mathf.Clamp(indexerPos, 0, Text.Length);

					textMesh.text = Text;
					if(isPassword){
						textMesh.text = new string('*',textMesh.text.Length);
					}
					if(textMesh.renderer.bounds.size.x > boxCollider.bounds.size.x)
					{
						var width = 0;
						var i = 0;
						int removedFromStart = 0;
						int removedFromEnd = 0;
						string newText = Text;
						if(isPassword){
							textMesh.text = new string('*',newText.Length);
						}
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
					if(isPassword){
						textMesh.text = new string('*',textMesh.text.Length);
					}
					shownString = textMesh.text;

			}
		}
	}

	/// <summary>
	/// Do verifications when selected.
	/// </summary>
	void Update()
	{
		if(isSelected)
		{
			// If clicked remove field selection
			if(Input.GetMouseButtonDown(0))
			{
				var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				pos.z = 0;
				if(!boxCollider.bounds.Contains(pos)){
					Selected(this, System.EventArgs.Empty);
				}
			}

			//Show | Hide Indexer
			if(!isPassword && mobileKeyboard == null){
				indexerTimer -= Time.deltaTime;

				if(indexerTimer < 0)
				{
					showIndexer = !showIndexer;
					indexerTimer = indexerTime;
					textMesh.text = Text.Insert(indexerPos,(showIndexer ? "|" : ""))
						.Substring(shownStartIndex, shownString.Length + (showIndexer ? 1 : 0));
				}
			}
		}
	}

	/// <summary>
	/// Raises the enable event.
	/// Sign components events
	/// </summary>
	void OnEnable()
	{
		tapGesture.Tapped += Selected;
	}

	/// <summary>
	/// Raises the disable event.
	/// Unsign components events.
	/// </summary>
	void OnDisable()
	{
		tapGesture.Tapped -= Selected;
		
	}
	#endregion

	#region Events
	/// <summary>
	/// Raises when enter is pressed.
	/// </summary>
	void EnterPressed ()
	{
		if(OnEnterPressed != null)
		{
			OnEnterPressed();
		}else{
			Selected (this, System.EventArgs.Empty);
		}
	}

	/// <summary>
	/// Selected toggle
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
	public void Selected (object sender, System.EventArgs e)
	{

		isSelected = !isSelected;
		SetInfo (isSelected);
	}

	/// <summary>
	/// Sets the info according to selected value.
	/// </summary>
	/// <param name="selected">If set to <c>true</c> selected.</param>
	void SetInfo (bool selected)
	{
		if (selected) {
#if !UNITY_EDITOR
			TouchScreenKeyboard.hideInput = false;
			mobileKeyboard = TouchScreenKeyboard.Open (Text, type, false, false, isPassword, false, defaultText);
#endif

			textMesh.color = selectedColor;
			indexerPos = Text.Length;
		}
		else {
			textMesh.color = normalColor;
			string newText = Text;
			#if !UNITY_EDITOR
			mobileKeyboard.active = false;
			mobileKeyboard = null;
			#endif

			while(GetTextWidth(newText) > boxCollider.bounds.size.x)
			{
				newText = newText.Remove(newText.Length - 1, 1);
			}
			textMesh.text = newText;
			if(isPassword){
				textMesh.text = new string('*',textMesh.text.Length);
			}
			if (string.IsNullOrEmpty (textMesh.text)) {
				textMesh.text = defaultText;
			}
		}
	}
	#endregion


	#region TextWrapper
	private Hashtable dict; //map character -> width

	new private Renderer renderer;
	string lastText = "";
	public void Start ()
	{
		textMesh = GetComponent<TextMesh> ();
		renderer = textMesh.renderer;
		collider = GetComponent<BoxCollider2D> ();
		dict = new Hashtable ();
		getSpace ();
		
	}
	
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
	


	#endregion

}

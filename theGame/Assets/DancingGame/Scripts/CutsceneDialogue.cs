using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class CutsceneDialogue : MonoBehaviour
{
    // Nour--Yellow, Crush--Green, DJ---Orange, Kissing Couple--cyan
    // Nour:0 Crush:1 DJ:2 Couple:3
    public Color[] colors = new Color[4];

    public TMP_Text txt;

    private string[] lines ={"","(This is it.)",
        "(Tonight is the night.)",
        "(This is the culmination of the last three years.)",
        "(All the afterschool dance classes... the suit fittings... the times spent practicing in front of the mirror... )",
        "(Tonight is the night I impress her.)",
        "",
        "(The plan is perfect. First I dazzle her with my flawless dance moves...)",
        "(...Then I walk up to her and say...)",
        "(\"Hey. My name is Nour. We were together in Algebra!\")",
        "",
        "(This is gonna work so well.)",
        "(I just need her to look at me. Why isn't she looking?)",
        "(Maybe she's distracted. There's so much else going on here tonight...)",
        "",
        "Hey!",
        "Hi~",
        "I really like your disembodied head.",
        "I like yours too...",
        "What are you doing out here?",
        "This dude asked me to dance…",
        "",
        "Oh my god! Hey! You made it!",
        "",
        "Yeah, I'm having a good time. How about you?",
        "Did you try this punch? I think someone put... Nutmeg in it! I know!!",
        "What's that? ",
        "",
        "Oh wow. Yeah. They're really tearing it up over there.",
        "That's Nour, right? Huh...",
        "(Ohmygodshe'slookingbenormal.)",
        "I'm really impressed with them. Honestly. They're *really* good.",
        "Hey, is this a remix? It doesn't sound like I remember it.",
        "",
        "The Spring Formal is my olympics.",
        "",
        "It is my mission in this life to bring the ecstatic power of music to these Middle Schoolers.",
        "",
        "When the beat drops it will also bring world peace.",
        "",
        "Omg, so what did you say???",
        "I don’t really like to dance.",
        "I don’t either.",
        "And I don’t really like him…",
        "I actually um…",
        "I have a crush on you…",
        "OMG me too!!!",
        "",
        "There's a lot going on at this party. Have you heard that Micah got kidnapped by aliens?",
        "I know! It's like, good for him! He had all those \"I want to believe\" posters on his locker.",
        "",
        "They're not slowing down, huh?",
        "Yeah... I've gotta say, I will never see Nour the same way again.",
        "(She keeps looking at me... That means it's working, right???)",
        "(Ok, don't think about it, don't think about it.)",
        "(Remember our master's words.. They always said...)",
        "(\"...If you think about impressing your crush it will not work so try not to think about it Nour OK\")",
        "",
        "(I'm getting kind of hot. I think I'm gonna take this off.)",
        "",
        "When I am behind these turn tables I feel certain I will never die!",
        "",
        "Alert! Alert! This breakdown will inject self confidence into the veins of the youth in this room.",
        "",
        "I want to recreate the dancing plague of 1518 with these beats!!!!",
        "",
        // "When I am behind these turn tables I feel certain I will never die!",
        // "",
        // "Alert! Alert! This breakdown will inject self confidence into the veins of the youth in this room.",
        // "",
        // "I want to recreate the dancing plague of 1518 with these beats!!!!", 
        // "",
        "*smack slurp smack slurp slurp slurp*",
        "",
        "(Ok... Ok Nour. The song is almost over.)",
        "(I'm gonna go talk to her... As soon as it's done.)",
        "",
        "(I think taking off the jacket was a good move. I've been working out. I'm sure it--)",
        "(--Wait. Who is that with her?)",
        "Oh, you want me to help you up so you can see better? Sure.",
        "",
        "There we go. ",
        "Aww. You look so adorable tonight.",
        "I love what this lipstick is doing for you.",
        "",
        "Give me a kiss.",
        "",
        "(I can't stay here.)"};

    private int index = 0;
    void Start()
    {
        txt.text = lines[index];
    }

    // Update is called once per frame
    void Update()
    {
        txt.text = lines[Mathf.Min(index, lines.Length - 1)];
    }

    public void Awake()
    {
        index += 1;
    }
    public void AdvanceTextColor(int index)
    {
        txt.color = colors[index];
    }
    public void AdvanceText()
    {
        index += 1;
    }

    public void ResetText()
    {
        index = 0;
    }

    public void SetText(int i)
    {
        index = i;
    }

}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yarn.Unity;

public class ComplimentManager : MonoBehaviour
{
    private InMemoryVariableStorage variableStorage;
    private string compliment;
    
    //UPDATE THESE LISTS to make sure every word is processed well
    private string[] adjectives={"phenomenal","fabulous","lush"};
    private string[] things={"smile","walk","spirit"};
    private string[] qualities={"stench","beauty","curvature"};
    private string[] comparedTo={"a tree","my mom","the Sun"};

    private List<string> words;

    void Start()
    {
        variableStorage = GameObject.FindObjectOfType<InMemoryVariableStorage>();
        words=new List<string>();
        words.Add("stench");
        words.Add("walk");
        words.Add("fabulous");
        words.Add("the Sun");
        //words.Add("phenomenal");
        //words.Add("lush");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return)){

            ComplimentEnd();
        }
    }


    void ComplimentEnd(){
        GenerateCompliment();
        variableStorage.SetValue("$compliment",compliment);
        FindObjectOfType<GameManager>().ComplimentEnd();
    }

    //Function for generating compliment text from words picked
    void GenerateCompliment(){
        //MAKE COMPLIMENT FROM TEMPLATES
        compliment="Your stench is so beautiful it makes me want to cry.";

        List<string> qs=new List<string>(); //quality
        List<string> ts=new List<string>(); //things being complimented
        List<string> ads=new List<string>();//adjectives
        List<string> cs=new List<string>(); //things compared to

        foreach(string w in words){
            if(qualities.Contains(w)){
                qs.Add(w);
            }else if(things.Contains(w)){
                ts.Add(w);
            }else if(adjectives.Contains(w)){
                ads.Add(w);
            }else if(comparedTo.Contains(w)){
                cs.Add(w);
            }
        }

        //grouping multiple words into "x, y and z" structure
        string q=GroupWords(qs);
        string t=GroupWords(ts);
        string a=GroupWords(ads);
        string c=GroupWords(cs);

        //generate compliment from structure
        if(q.Length>0){
            if(t.Length>0){
                if(a.Length>0){
                    if(c.Length>0){
                        compliment=Choose<string>("I love the "+q+" of your "+t+", it is so "+a+", like "+c+".",
                        "Your "+t+" is really "+a+". Especially its "+q+"! It reminds me of "+c+".");
                    }else{
                        compliment="I can't get enough of the "+q+" of your "+t+". It's very "+a+".";
                    }
                }else{
                    if(c.Length>0){
                        compliment="The "+q+" of your "+t+" is so "+a+".";
                    }else{
                        compliment="I'm obsessed with the "+q+" of your "+t+".";
                    }
                }
            }else{
                if(a.Length>0){
                    if(c.Length>0){
                        compliment="Your "+q+" is so "+a+"! It reminds me of "+c+".";
                    }else{
                        compliment=compliment="Your "+q+" is so "+a+"! I love it.";
                    }
                }else{
                    if(c.Length>0){
                        compliment="Thinking about your "+q+", I can't help but picture "+c+"!";
                    }else{
                        compliment="You have such "+q;
                    }
                }
            }
        }else{
            if(t.Length>0){
                if(a.Length>0){
                    if(c.Length>0){
                        compliment="Your "+t+" is so "+a+", it reminds me of "+c+".";
                    }else{
                        compliment="Your "+t+" is absolutely "+a+".";
                    }
                }else{
                    if(c.Length>0){
                        compliment="Your "+t+" is basically like "+c+".";
                    }else{
                        compliment=Choose<string>("I love your "+t+".","I'm obsessed with your "+t+".",
                        "I can't stop thinking about your "+t+".");
                    }
                }
            }else{
                if(a.Length>0){
                    if(c.Length>0){
                        compliment="You're so "+a+"! Kind of like "+c+"?!";
                    }else{
                        compliment=Choose<string>("You're so "+a+".","I love how you're "+a+".");
                    }
                }else{
                    if(c.Length>0){
                        compliment=Choose<string>("You're kind of like..."+c+".","You remind me of "+c+".");
                    }else{
                        compliment=Choose<string>("I like you... very much.","I like you.","You're ok.");
                    }
                }
            }
        }
        
    }

    private string GroupWords(List<string> l){
        string s="";
        for(int i=0;i<l.Count;i++){
            s=s+l[i];
            if(i<l.Count-1){
                if(i==l.Count-2){
                    s=s+" and ";
                }else{
                    s=s+", ";
                }
            }
        }
        return s;
    }

    public T Choose<T>(T a, T b, params T[] p){
        int random = Random.Range(0, p.Length + 2);
        if (random == 0) return a;
        if (random == 1) return b;
        return p[random - 2];
    }
}

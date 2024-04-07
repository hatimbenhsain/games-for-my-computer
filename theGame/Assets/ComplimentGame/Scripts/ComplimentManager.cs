using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yarn.Unity;

public class ComplimentManager : MonoBehaviour
{
    private InMemoryVariableStorage variableStorage;
    private string compliment;
    
    public GameObject ball;
    public GameObject hole;
    
    //UPDATE THESE LISTS to make sure every word is processed well
    private string[] adjectives={"phenomenal", "fabulous", "lively", "lush", "brilliant", "dazzling", "enchanting", "efficent", "exuberant", "fearless", "glimmering", "sparkling", "glowing", "inventive", "tantalizing", "kooky", "magnificent", "modest", "perfect", "powerful", "sleek", "splendid", "unique", "vibrant", "boundless", "colossal", "cosmopolitian", "far-flung", "fresh", "immaculate", "mysterious", "mystical", "prosperous", "superb", "enthusiastic", "lovely", "moldy", "grainy", "textured", "grim", "growing", "flickering", "twisting", "cloudy", "limp", "everlasting", "obscure", "niche", "kafkaesque", "avantgarde", "kind", "fragmented", "picturesque", "grandiose"};
    private string[] things={"smile", "walk", "spirit", "shirt", "dress", "socks", "eyebrows", "toenails", "earlobes", "watch", "eyes", "dog", "heart", "wrinkles", "freckles", "mole", "nostrils", "veins", "hair follicles", "energy", "stench", "shoes", "pants", "headband", "jewelry", "love", "tongue", "kneecaps", "fingers", "thumbs", "chin", "brain", "outfit", "vibes", "sentience", "backpack", "foot", "lamp", "Pet", "cuticles", "nostrils", "tummy", "shin", "appendix", "elbow", "inner ear ", "nose hair ", "feelings", "collar bone", "left calf "};
    private string[] qualities={"stench", "beauty", "wetness", "size", "curve", " ", "shape", "curvature", "intelligence", "resilience", "quality", "texture", "sound", "strength", "ambience", "Agon", "layers", "courage", "etherealness", "vision", "vibe", "smell", "wobbliness", "art style", "constitution", "aura", "thickness ", "flexibilty ", "pace", "length ", "value"};
    private string[] comparedTo={"a tree", "a dog", "the sea", "the sky", "my Mom", "the police", "King Arthur", "Napoleon", "cherry oak", "a Kinder Surprise", "a cigarette", "the Earth", "my phone", "an old guy", "climate change", "a dragon", "a tiger", "Naruto", "Goku", "Lara Croft", "Super Mario", "Batman", "Western medicine", "the market", "Web2", "a gif", "the Sun", "Spring", "Winter", "rain", "a cockroach", "tentacles", "goats", "jet fuel", "the quadratic formula", "chilly breeze", "butterfly wings", "untold secrets", "plucked flower", "midwest salad", "God", "watermelon", "fire", "a machete", "Willem Dafoe", "my neighbour's dog", "a comet ", "an eclipse ", "doors", "warm summer breeze ", "ocean mist"};

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
        // Check if the ball and hole are colliding
        if (AreColliding(ball, hole))
        {
            // Call the ComplimentEnd function when the collision is detected
            ComplimentEnd();
        }
        
        bool AreColliding(GameObject ball, GameObject hole)
        {
            // This method assumes both objects have colliders and returns true if they are intersecting
            Collider ballCollider = ball.GetComponent<Collider>();
            Collider holeCollider = hole.GetComponent<Collider>();
            if (ballCollider != null && holeCollider != null)
            {
                return ballCollider.bounds.Intersects(holeCollider.bounds);
            }
            return false;
        }
        
        if(Input.GetKeyDown(KeyCode.Return)){

            ComplimentEnd();
        }
    }


   public void ComplimentEnd(){
        GenerateCompliment();
        variableStorage.SetValue("$compliment",compliment);
        FindObjectOfType<GameManager>().ComplimentEnd();
    }

    //Function for generating compliment text from words picked
    void GenerateCompliment(){
        //MAKE COMPLIMENT FROM TEMPLATES
        //compliment="Your stench is so beautiful it makes me want to cry.";

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

    public void AddWord(string w){
        words.Add(w);
    }
}

using System.Collections.Generic;

namespace Project3
{
    public class State
    {
        public int PromptId { get; set; }

        public string Prompt1 { get; set; }
        public int Prompt1NextId { get; set; }

        public string Prompt2 { get; set; }
        public int Prompt2NextId { get; set; }

        public string Prompt3 { get; set; }
        public int Prompt3NextId { get; set; }
    }

    public static class Journey
    {
        static readonly Dictionary<int, State> states = new Dictionary<int, State>();

        static Journey()
        {
            states[0] = new State
            {
                PromptId = 0,
                Prompt1 = "Bacon ipsum dolor amet leberkas in occaecat, sunt consequat ham hock bacon exercitation.",
                Prompt1NextId = 1,
                Prompt2 = "Leverage agile frameworks to provide a robust synopsis for high level overviews.",
                Prompt2NextId = 2,
                Prompt3 = "Since then he had been skulking along towards Maybury, in the hope of getting out of danger Londonward.",
                Prompt3NextId = 3,
            };

            states[1] = new State
            {
                PromptId = 1,
                Prompt1 = "Do tenderloin cupim deserunt in. Sunt andouille id, burgdoggen sirloin anim pork chop.",
                Prompt1NextId = 11,
                Prompt2 = "Iterative approaches to corporate strategy foster collaborative thinking to further the overall value proposition.",
                Prompt2NextId = 22,
                Prompt3 = "People were hiding in trenches and cellars, and many of the survivors had made off towards Woking village and Send.",
                Prompt3NextId = 33,
            };

            states[2] = new State
            {
                PromptId = 2,
                Prompt1 = "Lorem ham pork, labore reprehenderit enim meatball biltong laborum minim nulla excepteur culpa.",
                Prompt1NextId = 11,
                Prompt2 = "Organically grow the holistic world view of disruptive innovation via workplace diversity and empowerment.",
                Prompt2NextId = 22,
                Prompt3 = "He had been consumed with thirst until he found one of the water mains near the railway arch smashed, and the water bubbling out like a spring upon the road.",
                Prompt3NextId = 33
            };

            states[3] = new State
            {
                PromptId = 3,
                Prompt1 = "Capicola picanha exercitation, in ut tail prosciutto bacon veniam strip steak doner irure voluptate chuck.",
                Prompt1NextId = 111,
                Prompt2 = "At the end of the day, going forward, a new normal that has evolved from generation X is on the runway heading towards a streamlined cloud solution.",
                Prompt2NextId = 222,
                Prompt3 = "That was the story I got from him, bit by bit.",
                Prompt3NextId = 33
            };

            states[11] = new State
            {
                PromptId = 11,
                Prompt1 = "Enim reprehenderit salami ut.",
                Prompt1NextId = 111,
                Prompt2 = "User generated content in real-time will have multiple touchpoints for offshoring.",
                Prompt2NextId = 2,
                Prompt3 = "He grew calmer telling me and trying to make me see the things he had seen.",
                Prompt3NextId = 33,
            };

            states[22] = new State
            {
                PromptId = 22,                
                Prompt1 = "Chicken excepteur aute beef.",
                Prompt1NextId = 1,
                Prompt2 = "Capitalise on low hanging fruit to identify a ballpark value added activity to beta test.",
                Prompt2NextId = 222,
                Prompt3 = "He had eaten no food since midday, he told me early in his narrative, and I found some mutton and bread in the pantry and brought it into the room. ",
                Prompt3NextId = 3
            };


            states[33] = new State
            {
                PromptId = 33,                
                Prompt1 = "Sirloin sint pork belly consequat, in ham hock pork chop t-bone sausage aliqua qui venison eu frankfurter.",
                Prompt1NextId = 1,
                Prompt2 = "Override the digital divide with additional clickthroughs from DevOps.",
                Prompt2NextId = 22,
                Prompt3 = "We lit no lamp for fear of attracting the Martians, and ever and again our hands would touch upon bread or meat.",
                Prompt3NextId = 333
            };

            states[111] = new State
            {
                PromptId = 111,
                Prompt1 = "Swine dolore pariatur consequat chicken excepteur in ut turkey fugiat, minim mollit dolore frankfurter.",
                Prompt1NextId = 0,
                Prompt2 = "Nanotechnology immersion along the information highway will close the loop on focusing solely on the bottom line.",
                Prompt2NextId = 0,
                Prompt3 = "As he talked, things about us came darkly out of the darkness, and the trampled bushes and broken rose trees outside the window grew distinct.",
                Prompt3NextId = 0
            };

            states[222] = new State
            {
                PromptId = 222,                
                Prompt1 = "Ipsum dolor fatback, ham brisket cillum andouille leberkas laborum deserunt in cupim exercitation shoulder.",
                Prompt1NextId = 0,
                Prompt2 = "Podcasting operational change management inside of workflows to establish a framework.",
                Prompt2NextId = 0,
                Prompt3 = "It would seem that a number of men or animals had rushed across the lawn.",
                Prompt3NextId = 0
            };

            states[333] = new State
            {
                PromptId = 333,                
                Prompt1 = "Meatloaf andouille nisi, jerky tongue tempor reprehenderit.",
                Prompt1NextId = 0,
                Prompt2 = "Taking seamless key performance indicators offline to maximise the long tail.",
                Prompt2NextId = 0,
                Prompt3 = "I began to see his face, blackened and haggard, as no doubt mine was also.",
                Prompt3NextId = 0
            };
        }

        public static State GetState(int id)
        {
            return states[id];
        }
    }
}

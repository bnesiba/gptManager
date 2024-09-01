Mostly a playground project for me to mess around with chatGPT and tool integrations.   

This application uses `bcn.ActionFlow`, a custom redux-like architecture I set up to smoothly integrate new features, handle async behaviors, and manage states.
The project is currently a C# .Net WebAPI but the use of ActionFlow means that almost all of the code could be moved to any C# .Net project type (MAUI for example) with very few changes necessary. 

ChatSessionFlow handles the main chat loop logic, while tool-state is maintained in ToolManagementFlow so future projects can define and use any tools they need. 
   
Currently integrates: ChatGPT, Google Cloud Platform, Google programmable search*, wikipedia api*   
*in abandoned branch - future integrations planned, but maybe unnecessary.

**There are 2 main Applications in the platform so far:**

1.) Tool-informed Chatbot
   - This is the default chatbot flow.
   - Using the ChatController and the ChatSessionFlow, this application implements a simple RAG chatbot with some basic tools.   
*This Flow was originally designed to use chatgpt-3.5 as often as possible due to cost concerns, I have since switched to chatgpt-4o-mini making some tools obsolute.*
        - **SendEmail** tool sends emails on the user's (my) behalf using the Google Cloud Platform.
        - **ImageEvaluate** tool allows the chatbot to pass images to chatgpt 4 to analyse them. *This allowed the chatbot to use chatgpt 3.5 most of the time while maintaining feature access
        - **NewsSearch** tool is a stub for a tool that could get and manage results from external news sources. At the moment it always returns fake articles about moon-aliens.
        - **KnownInformationSearch** tool allows the chatbot to ask itself knowledge questions. *This solved an issue with chatgpt-3.5 where it would keep trying tools instead of using internal knowledge. chatgpt-4o-mini does not appear to struggle with this.

2.) Children's Story Evaluator
   - Created as a demonstration of how one might use tools to extract, store, and search information from input documents (in this case children's stories)
   - Using StoryEvaluationController to trigger StoryEvaluatorFlow, this project defines two "features" or endpoints:
        - **Story Evaluate:** This endpoint takes in the full-text of a childrens story and produces+stores an object containing metadata extracted from the story including title/author(s), characters(with descriptions and roles, and tags related to the content of the story like vehicles, animals, or professions mentioned.
        - **Story Search:** This endpoint takes a prompt and, using the defined search tools, finds stories that match the user's query. It can also run the evaluation process on new stories it is given.
             - This is a fully-functional chatbot and can reason about stories, produce new stories from known content and anything else you would expect chatgpt to be capable of doing.
   - StoryEvaluationFlow defines two sets of tools, one for each of the two features.
        - **Story Evaluation** is split up across 4 tools. This allows the agent to use previously location information to inform later responses, and minimizing the chance for hallucinations. Each tool also includes an optional parameter for the agent to input any complications encountered while finding the associated information (maybe a story doesn't have an author for example), further minimising hallicinations and alerting the user if they occurred. 
             - **SetGeneralInfo** tool has the agent extract the title and author(s) of the particlar work.
             - **SetCharacterList** tool has the agent extract a list of character names, descriptions, and roles from the story.
             - **SetStoryTags** tool has the agent assign a number of tags to the story based on events or elements of the story (vehicles/animals/professions/family members/etc).
             - **SetStorySummary** tool is always run last and has the agent generate a short summary of the story. Because this tool is always run last, it benefits from the accumulated information from previously run tools.
        - **Story Search** only uses 2 tools for the story-search-chat *If this were a prod feature, there would probably be a couple more*
            - **SearchForStories** tool searches previously evaluated stories to retreive relevant stories for the end-user
            - **EvaluateNewStory** tool allows the bot to trigger the evaluation flow for a new story. This allows the chat to take in new stories (or to write new ones based on existing data) and store them for later search.

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.6.2

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Linq;
using Microsoft.Bot.Builder.AI.QnA;

namespace NewBot1.Bots
{
    public class EchoBot : ActivityHandler
    {

        public QnAMaker EchoBotQnA { get; private set; }
        public EchoBot(QnAMakerEndpoint endpoint)
        {
            // connects to QnA Maker endpoint for each turn
            EchoBotQnA = new QnAMaker(endpoint);
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            // First send the user input to your QnA Maker knowledge base
            if (turnContext.Activity.Text.ToString().ToUpper() == turnContext.Activity.Text.ToString())
            {
                await turnContext.SendActivityAsync("JGBot responds: DON'T TYPE IN ALL CAPS, IT MAKES MY EARS HURT");
            }
            else 
            {
                await AccessQnAMaker(turnContext, cancellationToken);
            }
            
           
        }

        // ******  Make Changes To State Management  ******
        //Sets and  classes to manage states
        private BotState _conversationState;
        private BotState _userState;
        
        void /*make public*/ StateManagementBot(ConversationState conversationState, UserState userState)
        {
            _conversationState = conversationState;
            _userState = userState;
        }

        //Sends message when a member is added to the conversation
        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(CreateActivityWithTextAndSpeak($"Hello João Guimarães. Ask me anything!"), cancellationToken);
                }
            }
        }


        //Accesses QnAMaker and Responds
        private async Task AccessQnAMaker(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var results = await EchoBotQnA.GetAnswersAsync(turnContext);
            if (results.Any())
            {
                await turnContext.SendActivityAsync(MessageFactory.Text("JGBot responds: " + results.First().Answer), cancellationToken);
            }
            else
            {
                await turnContext.SendActivityAsync(MessageFactory.Text("What you just wrote doesn't make any sense to me. Sorry man :("), cancellationToken);
            }
        }

        //LUIS setup
        public class BookingDetails
        {
            public string Destination { get; set; }

            public string Origin { get; set; }

            public string TravelDate { get; set; }
        }


        //Making Cards
        /*
        private async Task DisplayOptionsAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            //Create hero card
        }
        */


        //Makes speech messages readable by the bot and makes the bot be able to speak
        private IActivity CreateActivityWithTextAndSpeak(string message)
        {
            var activity = MessageFactory.Text(message);
            string speak = @"<speak version='1.0' xmlns='https://www.w3.org/2001/10/synthesis' xml:lang='en-US'>
              <voice name='Microsoft Server Speech Text to Speech Voice (en-US, JessaRUS)'>" +
              $"{message}" + "</voice></speak>";
            activity.Speak = speak;
            return activity;
        }
    }
}

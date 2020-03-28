import React from 'react';
import Config from '../Config'
import Non200ResponseError from '../Non200ResponseError'
import './InRoomComponent.css'

class InRoomComponent extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      name: null,
      numberOfPeopleInRoom: null,
      numberOfChameleonsInRoom: null,
      roomState: null,
      character: null,
      showStartGameButton: null,
      firstPersonName: null,
      timeToPollMillisecond: null,
      people: [],
      everyoneVoted: false,
      lastStatusHash: null,
      polling: true,
      timeOfLastChangeUtc: null,
    };

    this.poll = this.poll.bind(this);
    this.startGame = this.startGame.bind(this);
    this.leaveRoom = this.leaveRoom.bind(this);
    this.voteChange = this.voteChange.bind(this);

    this.poll();
  }

  poll() {
    const tenMinutes = 10 * 60;
    if (
      this.state.timeOfLastChangeUtc
      && this.nowAsUnixTimestampUtc() - this.state.timeOfLastChangeUtc > tenMinutes
    ) {
      this.setState({ polling: false, roomOld: true });
      return;
    }
    if (!this.state.polling) {
      return;
    }
    fetch(Config.backendBaseApiUrl() + 'room-status/?RoomCode=' + this.props.roomCode, {
      method: 'GET',
      headers: {
        'Accept': 'application/json',
        [Config.personIdHeader()]: this.props.personId,
      }
    })
      .then(response => {
        if (response.status !== 200) {
          throw new Non200ResponseError(response);
        }
        return response.json();
      })
      .then(jsonBody => {
        if (this.state.lastStatusHash !== jsonBody.Hash) {
          this.setState({timeOfLastChangeUtc: this.nowAsUnixTimestampUtc()});
        }
        this.setState({
          name: jsonBody.Name,
          numberOfPeopleInRoom: jsonBody.PeopleCount,
          numberOfChameleonsInRoom: jsonBody.ChameleonCount,
          roomState: jsonBody.State,
          character: jsonBody.Character,
          showStartGameButton: jsonBody.ShowStartGameButton,
          firstPersonName: jsonBody.FirstPersonName,
          timeToPollMillisecond: jsonBody.TimeToPollMillisecond,
          people: jsonBody.PeopleInRoom,
          everyoneVoted: jsonBody.EveryoneVoted,
          lastStatusHash: jsonBody.Hash,
        });
        if (jsonBody.TimeToPollMillisecond) {
          setTimeout(this.poll, jsonBody.TimeToPollMillisecond);
        }
      })
      .catch((error) => {
        if (error instanceof Non200ResponseError) {
          let status = error.response.status;
          if (status === 404 || status === 403) {
            this.props.onRoomLeft()
            return;
          }
        }
        console.error(error);
        // If we've had at least one successful response, and so this error
        // was probably just a one off, still try again.
        if (this.state.timeToPollMillisecond) {
          setTimeout(this.poll, this.state.timeToPollMillisecond);
        }
      })
  }

  startGame(e) {
    fetch(Config.backendBaseApiUrl() + 'start-game/?RoomCode=' + this.props.roomCode, {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        [Config.personIdHeader()]: this.props.personId,
      }
    })
    .catch((error) => { console.error(error); });
  }

  leaveRoom(e) {
    this.setState({polling: false});
    fetch(Config.backendBaseApiUrl() + 'leave-room/?RoomCode=' + this.props.roomCode, {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        [Config.personIdHeader()]: this.props.personId,
      }
    })
    .then(() => this.props.onRoomLeft())
    .catch((error) => { console.error(error); });
  }
  
  nowAsUnixTimestampUtc() {
    return Math.floor((new Date()).getTime() / 1000);
  }

  welcomeMessage() {
    if (this.state.numberOfPeopleInRoom === 1) {
      return <span>You've created a new room! To play a game tell your friends to open this page on their phones and use room code '{this.props.roomCode}'</span>
    } else if(this.state.numberOfPeopleInRoom < 3) {
      return <span>Keep inviting - You need at least 3 people to play a game!</span>
    } else {
      return <span>There's {this.state.numberOfPeopleInRoom} of you ready to play.<br/>Once everyone's in, click Start Game</span>
    }
  }

  whatToDo() {
    let chameleonCount = this.state.numberOfChameleonsInRoom;
    if (this.state.character === "chameleon") {
      let brackets = "";
      if (chameleonCount === 2) {
        brackets = " (apart from the other chameleon)"
      } else if (chameleonCount > 2) {
        brackets = ` (apart from the other ${chameleonCount-1} chameleons)`
      }
      return <div>
        Everyone else{brackets} has been given the same word,
        they'll take it in turns to say a <i>single</i> word that relates to it.
        Try to blend in and hide the fact that you are a chameleon!
      </div>
    } else {
      let chameleonCountMessage = chameleonCount === 1
        ? "One of the others is a chameleon and doesn't know the word."
        : `${chameleonCount} of the others are chameleons who don't know the word.`
      return <div>
        Take it in turns to say a <i>single</i> word that relates to the word above. {chameleonCountMessage}
        <br />
        Don't make it too easy for them to blend in with everyone else.
      </div>
    }
  }

  orderOfPlay() {
    return <div className="InRoom-order_of_play">
      {`Player order: ${this.state.firstPersonName}, `}
      {this.state.people
        .filter(x => x.Name != this.state.firstPersonName)
        .map(person => person.Name)
        .join(", ")}
    </div>
  }

  mostVotesComparer(a, b) {
    if (a.Votes > b.Votes) {
      return -1;
    }
    if (b.Votes > a.Votes) {
      return 1;
    }
    return 0;
  }

  nameComparer(a, b) {
    return ('' + a.Name).localeCompare(b.Name);
  }

  votingComponent() {
    if (this.state.everyoneVoted) {
      return <div className="InRoom-voting">
        <div>The votes are in!</div>
        <ul>
          {this.state.people
            .sort(this.nameComparer)
            .sort(this.mostVotesComparer)
            .filter(x => x.Votes > 0)
            .map(person => {
              let message = person.Votes === 1
                ? `${person.Name} - ${person.Votes} Vote`
                : `${person.Name} - ${person.Votes} Votes`
              return (
                <li key={person.Id}>{message}</li>
              )
            })}
        </ul>
      </div>
    }
    if (this.state.character === "chameleon") {
      return <div className="InRoom-voting">Chameleons can't vote! Results will show once everyone else has voted.</div>
    }
    return <div className="InRoom-voting">
      <div>Choose someone you think is a Chameleon:</div>
      <ul>
        {this.state.people
          .sort(this.nameComparer)
          .filter(x => x.Id !== this.props.personId)
          .map(person => {
            let checkboxId = `checkbox-chameleon-guess-${person.Id}`
            return <li key={person.Id}>
              <input
                type="radio"
                name="chameleon-guess"
                value={person.Id}
                onChange={this.voteChange}
                id={checkboxId} />
              <label
                htmlFor={checkboxId} >
                {person.Name}
              </label>
            </li>
          })}
      </ul>
      <div>Results will show once everyone has voted.</div>
    </div>
  }

  voteChange(e) {
    fetch(Config.backendBaseApiUrl() + 'vote/?RoomCode=' + this.props.roomCode, {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        [Config.personIdHeader()]: this.props.personId,
      },
      body: JSON.stringify({
        Vote: e.target.value,
      })
    })
    .catch((error) => { console.error(error); });
  }

  render() {
    return (
      <div className="InRoom-game_area">
        {this.state.roomState === null ? (
          <div>Loading game...</div>
        ) : (
          <div>
            {this.state.roomOld && 
              <div>Still playing? <a href={window.location} className="Chameleon-link">Refresh</a></div>
            }
            <div className="InRoom-room_code">
              Room code: <span className="InRoom-room_code-code">{this.props.roomCode}</span>
              <br />
              Players: {this.state.numberOfPeopleInRoom}
            </div>
            {this.state.roomState === "PreGame" ? (
              <div>
                <div>
                  {this.welcomeMessage()}
                </div>
                {this.state.numberOfPeopleInRoom >= 3 && this.state.showStartGameButton && 
                  <button onClick={this.startGame}>Start Game</button>
                }
              </div>
            ) : (
              <div>
                <div>The word is</div>
                <div className="InRoom-character">
                  {this.state.character === "chameleon" ? "???" : this.state.character}
                </div>
                <div>
                  {this.state.character === "chameleon" && 
                    <i>You are a chameleon! Try and blend in</i>
                  }
                </div>
                
                {this.orderOfPlay()}

                {this.votingComponent()}
                
                <div className="InRoom-what_to_do">
                  {this.whatToDo()}
                </div>
                
                {this.state.showStartGameButton && (
                  <div className="InRoom-start_new_game">
                    <button onClick={this.startGame}>Start New Game</button>
                  </div>
                )}
              </div>
            )}
            <div className="InRoom-leave">
              <button onClick={this.leaveRoom}>Join a different room</button>
            </div>
          </div>
        )}
      </div>
    );
  }
}

export default InRoomComponent;

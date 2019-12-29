import React from 'react';
import Config from '../Config'

class InRoomComponent extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      name: null,
      numberOfPeopleInRoom: null,
      numberOfChameleonsInRoom: null,
      roomState: null,
      character: null,
      firstPersonName: null,
      timeToPollMillisecond: null,
      polling: true,
    };

    this.poll = this.poll.bind(this);
    this.startGame = this.startGame.bind(this);
    this.leaveRoom = this.leaveRoom.bind(this);

    this.poll();
  }

  poll() {
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
          throw {
            non200Response: true,
            response: response,
          };
        }
        return response.json();
      })
      .then(jsonBody => {
        this.setState({
          name: jsonBody.Name,
          numberOfPeopleInRoom: jsonBody.PeopleCount,
          numberOfChameleonsInRoom: jsonBody.ChameleonCount,
          roomState: jsonBody.State,
          character: jsonBody.Character,
          firstPersonName: jsonBody.FirstPersonName,
          timeToPollMillisecond: jsonBody.TimeToPollMillisecond,
        });
        if (jsonBody.TimeToPollMillisecond) {
          setTimeout(this.poll, jsonBody.TimeToPollMillisecond);
        }
      })
      .catch((error) => {
        if (error.non200Response) {
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
  
  render() {
    return (
      <div>
        {this.state.roomState === null ? (
          <div>Loading...</div>
        ) : (
          <div>
            Welcome {this.state.name} to room {this.props.roomCode}.
            <br />
            <div>There are {this.state.numberOfPeopleInRoom} people in the room, {this.state.numberOfChameleonsInRoom} of them are Chameleons!</div>
            {this.state.roomState === "PreGame" ? (
              <div>
                {this.state.numberOfPeopleInRoom < 3 ? (
                  <div>You need at least 3 people to play a game!</div>
                ) : (
                  <button onClick={this.startGame}>Start Game</button>
                )}
              </div>
            ) : (
              <div>
                {this.state.character === "chameleon" ? (
                  <div>You are a chameleon!</div>
                ) : (
                  <div>The word is '{this.state.character}'</div>
                )}
                <div>{this.state.firstPersonName} goes first</div>
                <button onClick={this.startGame}>Start New Game</button>
              </div>
            )}
            <button onClick={this.leaveRoom}>Leave Room</button>
          </div>
        )}
      </div>
    );
  }
}

export default InRoomComponent;

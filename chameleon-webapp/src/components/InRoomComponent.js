import React from 'react';

class InRoomComponent extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      numberOfPeopleInRoom: null,
      numberOfChameleonsInRoom: null,
      roomState: null,
      character: null,
    };

    this.poll = this.poll.bind(this);
    this.startGame = this.startGame.bind(this);

    this.poll();
  }

  poll() {
    fetch(process.env.REACT_APP_CHAMELEON_BACKEND_BASE_URL + '/api/room-status/?RoomCode=' + this.props.roomCode, {
      method: 'GET',
      headers: {
        'Accept': 'application/json',
        'X-Chameleon-PersonId': this.props.personId,
      }
    })
      .then(response => response.json())
      .then(jsonBody => {
        this.setState({
          numberOfPeopleInRoom: jsonBody.PeopleCount,
          numberOfChameleonsInRoom: jsonBody.ChameleonCount,
          roomState: jsonBody.State,
          character: jsonBody.Character,
        });
        if (jsonBody.TimeToPollMillisecond) {
          setTimeout(this.poll, jsonBody.TimeToPollMillisecond);
        }
      })
      .catch((error) => { console.error(error); })
  }

  startGame(e) {
    fetch(process.env.REACT_APP_CHAMELEON_BACKEND_BASE_URL + '/api/start-game/?RoomCode=' + this.props.roomCode, {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'X-Chameleon-PersonId': this.props.personId,
      }
    })
  }
  
  render() {
    return (
      <div>
        Welcome {this.props.personId} to room {this.props.roomCode}.
        <br />
        {this.state.roomState === null ? (
          <div>Loading...</div>
        ) : (
          <div>
            <div>There are {this.state.numberOfPeopleInRoom} people in the room, {this.state.numberOfChameleonsInRoom} of them are Chameleons!</div>
            {this.state.roomState === "PreGame" ? (
              <div>
                <button onClick={this.startGame}>Start Game</button>
              </div>
            ) : (
              <div>
                <div>The word is {this.state.character}</div>
                <button onClick={this.startGame}>Start New Game</button>
              </div>
            )}
          </div>
        )}
      </div>
    );
  }
}

export default InRoomComponent;

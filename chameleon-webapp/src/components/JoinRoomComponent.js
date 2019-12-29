import React from 'react';
import Config from '../Config'
import Non200ResponseError from '../Non200ResponseError'

class JoinRoomComponent extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      roomCodeInput: '' ,
      personNameInput: '',
      errorMessage: '',
    };

    this.handleRoomChange = this.handleRoomChange.bind(this);
    this.handlePersonChange = this.handlePersonChange.bind(this);
    this.handleRoomCodeKeyUp = this.handleRoomCodeKeyUp.bind(this);
    this.createRoom = this.createRoom.bind(this);
    this.joinRoom = this.joinRoom.bind(this);
    this.makeJoinRoomRequest = this.makeJoinRoomRequest.bind(this);
  }

  createRoom(e) {
    this.makeJoinRoomRequest(null, this.state.personNameInput);
  }

  joinRoom(e) {
    this.makeJoinRoomRequest(this.state.roomCodeInput, this.state.personNameInput);
  }

  makeJoinRoomRequest(roomCode, personName) {
    this.setState({ errorMessage: '' });
    fetch(Config.backendBaseApiUrl() + 'join-room/', {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        RoomCode: roomCode,
        PersonName: personName,
      })
    })
      .then(response => {
        if (response.status !== 200) {
          throw new Non200ResponseError(response);
        }
        return response.json();
      })
      .then(jsonBody => this.props.onRoomJoined(jsonBody.RoomCode, jsonBody.PersonId))
      .catch((error) => {
        if (error instanceof Non200ResponseError) {
          error.response.text().then(message => {
            this.setState({ errorMessage: message });
          });
          return;
        }
        console.error(error);
      })
  }

  handleRoomChange(event) {
    this.setState({roomCodeInput: event.target.value});
  }

  handlePersonChange(event) {
    this.setState({personNameInput: event.target.value});
  }

  handleRoomCodeKeyUp(event) {
    if (event.keyCode === 13) { // enter key
      this.makeJoinRoomRequest(this.state.roomCodeInput, this.state.personNameInput);
      event.preventDefault();
    }
  }

  render() {
    return (
      <div>
        <input
          type="text"
          value={this.state.personNameInput}
          onChange={this.handlePersonChange}
          placeholder="Enter your name" />
        <br/>
        <hr></hr>
        <button onClick={this.createRoom}>Create Room</button>
        <br /> or <br />
        <input
          type="text"
          value={this.state.roomCodeInput}
          onChange={this.handleRoomChange}
          onKeyUp={this.handleRoomCodeKeyUp}
          placeholder="Enter 4 letter room code" />
        <button onClick={this.joinRoom}>Join Room</button>
        <div>{this.state.errorMessage}</div>
      </div>
    );
  }
}

export default JoinRoomComponent;

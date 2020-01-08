import React from 'react';
import Config from '../Config'
import './JoinRoomComponent.css'
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
    this.handlePersonNameKeyUp = this.handlePersonNameKeyUp.bind(this);
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
    this.setState({ roomCodeInput: event.target.value.toUpperCase() });
  }

  handlePersonChange(event) {
    this.setState({ personNameInput: event.target.value });
  }

  handleRoomCodeKeyUp(event) {
    if (event.keyCode === 13) { // enter key
      this.joinRoom(event);
      event.preventDefault();
    }
  }

  handlePersonNameKeyUp(event) {
    if (event.keyCode === 13) { // enter key
      if (this.state.roomCodeInput === '') {
        this.createRoom();
      } else {
        this.joinRoom();
      }
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
          onKeyUp={this.handlePersonNameKeyUp}
          tabIndex="1"
          placeholder="Enter your name" />
        <br/>
        <hr/>
        <div>Already have a room code?</div>
        <input
          type="text"
          value={this.state.roomCodeInput}
          onChange={this.handleRoomChange}
          onKeyUp={this.handleRoomCodeKeyUp}
          tabIndex="2"
          className="JoinRoom-room_code_input"
          placeholder="Enter room code" />
        <button onClick={this.joinRoom} tabIndex="3">
          Join
        </button>

        <br/><br/>otherwise<br/>

        <button onClick={this.createRoom} tabIndex="4">
          Create a new room
        </button>
      
        <div className="error">{this.state.errorMessage}</div>
      </div>
    );
  }
}

export default JoinRoomComponent;

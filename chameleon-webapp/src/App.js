import React from 'react';
import './App.css';

class JoinRoomComponent extends React.Component {
  constructor(props) {
    super(props);
    this.state = { roomCode: '' };

    this.handleChange = this.handleChange.bind(this);
    this.createRoom = this.createRoom.bind(this);
    this.joinRoom = this.joinRoom.bind(this);
    this.makeJoinRoomRequest = this.makeJoinRoomRequest.bind(this);
  }

  createRoom(e) {
    this.makeJoinRoomRequest(null);
  }

  joinRoom(e) {
    this.makeJoinRoomRequest(this.state.roomCode);
  }

  makeJoinRoomRequest(roomCode) {
    fetch(process.env.REACT_APP_CHAMELEON_BACKEND_BASE_URL + '/api/join-room/', {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        roomCode: roomCode,
      })
    })
      .then(response => response.json())
      .then(jsonBody => this.props.onRoomJoined(jsonBody.RoomCode, jsonBody.PersonId))
      .catch((error) => { console.error(error); })
  }

  handleChange(event) {
    this.setState({roomCode: event.target.value});
  }

  render() {
    return (
      <div>
        <button onClick={this.createRoom}>Create Room</button>
        <br/>
        <button onClick={this.joinRoom}>Join Room</button>
        <input type="text" value={this.state.roomCode} onChange={this.handleChange} />
      </div>
    );
  }
}

class InRoomComponent extends React.Component {
  render() {
    return (
      <div>
        Welcome {this.props.personId} to room {this.props.roomCode}
      </div>
    );
  }
}

class ChameleonComponent extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      roomCode: null,
      personId: null
    };
    
    this.onRoomJoined = this.onRoomJoined.bind(this);
  }

  onRoomJoined(roomCode, personId) {
    this.setState({roomCode: roomCode, personId: personId});
  }

  render() {
    return (
      <div className="App">
        <header className="App-header">
          <p>
            Welcome to Chameleon!
          </p>
          <img
            src='https://image.shutterstock.com/image-vector/cartoon-lizard-on-branch-260nw-370250969.jpg'
            alt="Chameleon!"
          />
          <br />
          {(this.state.roomCode != null) ? <InRoomComponent roomCode={this.state.roomCode} personId={this.state.personId} /> : <JoinRoomComponent onRoomJoined={this.onRoomJoined} />}
        </header>
      </div>
    );
  }
}

function App() {

  return (
    <ChameleonComponent></ChameleonComponent>
  );
}

export default App;

import React from 'react';
import logo from './logo.svg';
import './App.css';

class JoinRoomComponent extends React.Component {
  constructor(props) {
    super(props);
    this.state = {roomCode: ''};

    this.handleChange = this.handleChange.bind(this);
    this.createRoom = this.createRoom.bind(this);
    this.joinRoom = this.joinRoom.bind(this);
  }

  createRoom(e) {
    this.makeJoinRoomRequest(null);
  }

  joinRoom(e) {
    this.makeJoinRoomRequest(this.state.roomCode);
  }

  makeJoinRoomRequest(roomCode) {
    fetch('https://chameleon.jacoblever.dev/api/join-room/', {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        roomCode: roomCode,
      })
    })
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


function App() {

  return (
    <div className="App">
      <header className="App-header">
        <p>
          Welcome to Chameleon!
        </p>
        <img src='https://image.shutterstock.com/image-vector/cartoon-lizard-on-branch-260nw-370250969.jpg'/>
        <br/>
        <JoinRoomComponent></JoinRoomComponent>
      </header>
    </div>
  );
}

export default App;

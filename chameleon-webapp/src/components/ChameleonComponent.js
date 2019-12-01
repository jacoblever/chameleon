import React from 'react';
import JoinRoomComponent from './JoinRoomComponent';
import InRoomComponent from './InRoomComponent';
import Cookies from 'js-cookie';

class ChameleonComponent extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      roomCode: Cookies.get('roomCode'),
      personId: Cookies.get('personId')
    };

    this.onRoomJoined = this.onRoomJoined.bind(this);
  }

  onRoomJoined(roomCode, personId) {
    Cookies.set('personId', personId, { expires: 1 });
    Cookies.set('roomCode', roomCode, { expires: 1 });
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

export default ChameleonComponent;

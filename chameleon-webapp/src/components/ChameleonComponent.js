import React from 'react';
import JoinRoomComponent from './JoinRoomComponent';
import InRoomComponent from './InRoomComponent';

class ChameleonComponent extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      roomCode: null,
      personId: null
    };

    var roomCode = this.readCookie("roomCode");
    var personId = this.readCookie("personId");
    if(roomCode) {
      this.state.roomCode = roomCode;
    }
    if(personId) {
      this.state.personId = personId;
    }

    this.onRoomJoined = this.onRoomJoined.bind(this);
  }

  onRoomJoined(roomCode, personId) {
    document.cookie = "personId=" + personId;
    document.cookie = "roomCode=" + roomCode;
    this.setState({roomCode: roomCode, personId: personId});
  }

  readCookie(cookieName) {
    var name = cookieName + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var cookieArray = decodedCookie.split(';');
    for(var i = 0; i < cookieArray.length; i++) {
      var cookieString = cookieArray[i];
      while (cookieString.charAt(0) === ' ') {
        cookieString = cookieString.substring(1);
      }
      if (cookieString.indexOf(name) === 0) {
        return cookieString.substring(name.length, cookieString.length);
      }
    }
    return null;
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

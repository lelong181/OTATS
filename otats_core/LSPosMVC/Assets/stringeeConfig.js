const PROJECT_ID = "SKx0Hef124QKRZlOUAmVVYn6pC0QXFdfJ";
const PROJECT_SECRET = "U1dEUWpnUjZpV3NNOTdkV0k3QU9PTTAxNEREOVdjYw==";
const BASE_URL = "https://api.stringee.com/v1/room2";

class API {
    constructor(projectId, projectSecret) {
        this.projectId = projectId;
        this.projectSecret = projectSecret;
        this.restToken = "";
    }

    async createRoom() {
        const roomName = Math.random().toFixed(4);
        //const response = await axios.post(
        //    `${BASE_URL}/create`,
        //    {
        //        name: roomName,
        //        uniqueName: roomName
        //    },
        //    {
        //        headers: this._authHeader()
        //    }
        //);
        //const room = response.data;
        //console.log({ room });
        //return room;
        return $.ajax({
            url: BASE_URL + '/create',
            type: 'POST',
            headers: {
                'X-STRINGEE-AUTH': this.restToken
            },
            data: JSON.stringify({
                name: roomName,
                uniqueName: roomName
            })
        })


    }

    async listRoom() {
        const response = await axios.get(`${BASE_URL}/list`, {
            headers: this._authHeader()
        });

        const rooms = response.data.list;
        console.log({ rooms });
        return rooms;
    }

    async deleteRoom(roomId) {
        //const response = await axios.put(`${BASE_URL}/delete`, {
        //    roomId
        //}, {
        //        headers: this._authHeader()
        //    })

        //console.log({ response })

        //return response.data;
        return $.ajax({
            url: BASE_URL + '/delete',
            type: 'DELETE',
            contentType:'application/json',
            headers: {
                'X-STRINGEE-AUTH': this.restToken
            },
            data: JSON.stringify({
                roomId: roomId
            })
        })
    }

    async clearAllRooms() {
        const rooms = await this.listRoom()
        const response = await Promise.all(rooms.map(room => this.deleteRoom(room.roomId)))

        return response;
    }

    async setRestToken() {
        const tokens = await this._getToken({ rest: true });
        const restToken = JSON.parse(tokens).rest_access_token;
        this.restToken = restToken;

        return restToken;
    }

    async getUserToken(userId) {
        const tokens = await this._getToken({ userId });
        return JSON.parse(tokens).access_token;
    }

    async getRoomToken(roomId) {
        const tokens = await this._getToken({ roomId });
        return JSON.parse(tokens).room_token;
    }

    async _getToken({ userId, roomId, rest }) {
        return $.ajax({
            url: 'https://v2.stringee.com/web-sdk-conference-samples/php/token_helper.php',
            type: 'GET',
            data: {
                keySid: this.projectId,
                keySecret: this.projectSecret,
                userId,
                roomId,
                rest
            }
        })
        //    .done(function successCallback(response) {
        //    const tokens = JSON.parse(response);
        //    return tokens;
        //})
        //const response = await axios.get(
        //    "https://v2.stringee.com/web-sdk-conference-samples/php/token_helper.php",
        //    {
        //        params: {
        //            keySid: this.projectId,
        //            keySecret: this.projectSecret,
        //            userId,
        //            roomId,
        //            rest
        //        }
        //    }
        //);

        //const tokens = response.data;
        //console.log({ tokens });
        //return tokens;
    }

    isSafari() {
        const ua = navigator.userAgent.toLowerCase();
        return !ua.includes('chrome') && ua.includes('safari');
    }

    _authHeader() {
        return {
            "X-STRINGEE-AUTH": this.restToken
        };
    }
}

const api = new API(PROJECT_ID, PROJECT_SECRET);

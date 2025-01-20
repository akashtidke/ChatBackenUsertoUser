# ChatBackenUsertoUser


Documentation of Tests


Testing Approach

1. Unit Testing
Objective: Validate individual functions or modules for expected behavior.
Scope:
- WebSocket message processing.
- REST API endpoints for message retrieval and sending.
- Data storage and retrieval logic in MessageStore.

2. Integration Testing
Objective: Ensure seamless interaction between the WebSocket server and REST APIs.
Scope:
- Real-time message delivery.
- API fallback for offline users.
- Consistency between WebSocket and REST API data.




3. Manual Testing
Objective: Verify the system's end-to-end functionality.
Scope:
- Use Postman for API requests.
- Employ WebSocket clients (e.g., websocat, Postman) to simulate connections.
- Confirm user experiences across various scenarios.

Test Cases

Unit Testing

WebSocket Message Processing
Test Case | Input | Expected Output
-----------------------------------------
Valid chat message | JSON message with `FromUserId`, `ToUserId`, `MessageContent` | Message is processed and sent to recipient.
Invalid JSON structure | Malformed JSON | Return error message: "Invalid message structure."

REST API Endpoints
Test Case | API Call | Expected Output
-----------------------------------------
Retrieve chat history | `/api/Message/{user1Id}/{user2Id}` | Return a list of messages in chronological order.
Retrieve non-existent history | `/api/Message/nonExistentUser1/nonExistentUser2` | Return `404 Not Found`.
Send valid chat message | POST `/api/Message/send-chat` | Store message and send it to online recipient via WebSocket.
Send message to offline user | POST `/api/Message/send-chat` | Store message and return `Recipient is not online`.

Integration Testing

Real-Time Message Delivery
Test Case | Input | Expected Output
-----------------------------------------
Online recipient receives message | WebSocket connection established, message sent via WebSocket | Message delivered to recipient instantly.

Multiple Connections
Test Case | Input | Expected Output
-----------------------------------------
Multiple WebSocket connections | Two WebSocket connections for one user | Both connections receive real-time messages simultaneously.

Edge Case Testing

Invalid Payloads
Test Case | Input | Expected Output
-----------------------------------------
Missing `FromUserId` | JSON message missing `FromUserId` | Return error message: "Message must contain FromUserId."
Missing `ToUserId` | JSON message missing `ToUserId` | Return error message: "Message must contain ToUserId."
Empty `MessageContent` | JSON message with empty `MessageContent` | Return error message: "Message must contain MessageContent."

User Disconnections
Test Case | Input | Expected Output
-----------------------------------------
User disconnects mid-session | WebSocket closes unexpectedly | Remove user connection from the active connection pool.



Manual Testing

Tools Used
- Postman: Test REST APIs for retrieving messages and sending chat messages.
- WebSocket Clients: Simulate multiple WebSocket connections and validate message delivery.

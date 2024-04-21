#version 430 core  
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoords;
//layout (location = 3) in vec3 offset;
layout (location = 3) in mat4 instanceMatrix;

uniform mat4 model;  
uniform mat4 view;  
uniform mat4 projection;

//uniform vec4 uColor;
//out vec4 inColor; // przeka¿ kolor do FS 
out vec2 TexCoords;
out vec3 Normal;
out vec3 Position;

void main()  
{  
    // Zauwa¿, ¿e czytamy mno¿enie macierzy od prawej do lewej  
    //gl_Position = projection * view * model * vec4(aPos + offset, 1.0f); 
    gl_Position = projection * view * model * instanceMatrix * vec4(aPos, 1.0f); 
    //inColor = uColor;  
    TexCoords = aTexCoords;  
    Normal = aNormal; 
    Position = vec3(model * instanceMatrix * vec4(aPos, 1.0f));
}
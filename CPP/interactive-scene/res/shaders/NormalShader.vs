#version 430 core  
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoords;

uniform mat4 model;  
uniform mat4 view;  
uniform mat4 projection;

//uniform vec4 uColor;
//out vec4 inColor; // przeka� kolor do FS 
out vec2 TexCoords;
out vec3 Normal;
out vec3 Position;

void main()  
{   
    gl_Position = projection * view * model * vec4(aPos, 1.0f); 
    //inColor = uColor;  
    TexCoords = aTexCoords;  
    //Normal = aNormal; 
    Normal = mat3(transpose(inverse(model))) * aNormal;
    Position = vec3(model * vec4(aPos, 1.0f));
}
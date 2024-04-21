#version 330 core
out vec4 FragColor;

in vec2 TexCoords;
in vec3 Normal;
in vec3 Position;

uniform float coefR;
uniform float coefG;
uniform float coefB;
uniform vec3 cameraPos;
uniform samplerCube skybox;

void main()
{             
    float ratioR = 1.00 / coefR;
    float ratioG = 1.00 / coefG;
    float ratioB = 1.00 / coefB;
    vec3 I = normalize(Position - cameraPos);
    vec3 RR = refract(I, normalize(Normal), ratioR);
    vec3 RG = refract(I, normalize(Normal), ratioG);
    vec3 RB = refract(I, normalize(Normal), ratioB);
    FragColor = vec4(texture(skybox, RR).r, texture(skybox, RG).g, texture(skybox, RB).b, 1.0);
}
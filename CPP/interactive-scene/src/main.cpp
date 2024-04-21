// dear imgui: standalone example application for GLFW + OpenGL 3, using programmable pipeline
// If you are new to dear imgui, see examples/README.txt and documentation at the top of imgui.cpp.
// (GLFW is a cross-platform general purpose library for handling windows, inputs, OpenGL/Vulkan graphics context creation, etc.)

#include "imgui.h"
#include "imgui_impl/imgui_impl_glfw.h"
#include "imgui_impl/imgui_impl_opengl3.h"
#include <stdio.h>

#define IMGUI_IMPL_OPENGL_LOADER_GLAD

#include <glad/glad.h>  // Initialize with gladLoadGL()

#include <GLFW/glfw3.h> // Include glfw3.h after our OpenGL definitions
#include <spdlog/spdlog.h>

#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>

#include <stb_image.h>
#include <Camera.h>
#include <Shader.h>
#include <GameObject.h>
#include <PlanePLayerController.h>
#include <CameraObject.h>
#include <PointLight.h>
#include <SpotLight.h>
#include <DirectionalLight.h>


void mouse_callback(GLFWwindow* window, double xposIn, double yposIn);

static void glfw_error_callback(int error, const char* description)
{
    fprintf(stderr, "Glfw Error %d: %s\n", error, description);
}


bool init(); 
void loadSkybox();
void drawSkybox();
void setUpScene();

void init_imgui();

void input();
void update();
void render();

void imgui_begin();
void imgui_render();
void imgui_end();

void end_frame();

void updateView();
void updateInstancesData();

void updatePointLightData();
void updateSpotLight1Data();
void updateSpotLight2Data();
void updateDirectionalLightData();
void updateAditionalLightData();

constexpr int32_t WINDOW_WIDTH  = 1920;
constexpr int32_t WINDOW_HEIGHT = 1080;

GLFWwindow* window = nullptr;

// Change these to lower GL version like 4.5 if GL 4.6 can't be initialized on your machine
const     char*   glsl_version     = "#version 430";
constexpr int32_t GL_VERSION_MAJOR = 4;
constexpr int32_t GL_VERSION_MINOR = 3;

/*bool   show_demo_window = true;
bool   show_another_window = false;/**/
ImVec4 clear_color         = ImVec4(0.4f, 0.4f, 0.8f, 1.0f);


//Camera* camera;
// timing
float deltaTime = 0.0f;	// time between current frame and last frame
float lastFrame = 0.0f;

bool moveView = true;

bool firstMouse = true;
float lastX = WINDOW_WIDTH / 2.0;
float lastY = WINDOW_HEIGHT / 2.0;

float viewRange = 3000.0f;

glm::mat4 model = glm::mat4(1.0f);
glm::mat4 view = glm::mat4(1.0f);
glm::mat4 projection = glm::mat4(1.0f);

Shader* normalShader;
Shader* instanceShader;
Shader* lightShader;
Shader* SkyboxShader;
Shader* reflectionShader;
Shader* refractionShader;

GameObject* rootGameObject;

Model* terrain;
Model* house;
Model* cone1;
Model* cone2;
Model* sphere;
Model* arrow;
Model* plane;

const int numberOfRows = 100;
const int numberOfColumns = 40;
InstancesData* instancesData[2];

PointLight* pointLight;
SpotLight* spotLight1;
SpotLight* spotLight2;
DirectionalLight* directionalLight;
float n = 4.0f;


unsigned int skyboxTextureID;
unsigned int skyboxVAO, skyboxVBO;
std::vector<std::string> faces = {
    "res/textures/right.jpg",
    "res/textures/left.jpg",
    "res/textures/top.jpg",
    "res/textures/bottom.jpg",
    "res/textures/front.jpg",
    "res/textures/back.jpg"
};

GameObject* Plane;
PlanePLayerController* Player;
CameraObject* cameraObj;


int main(int argc, char** argv)
{
    if (!init())
    {
        spdlog::error("Failed to initialize project!");
        return EXIT_FAILURE;
    }
    spdlog::info("Initialized project.");

    init_imgui();
    spdlog::info("Initialized ImGui.");

    //camera = new Camera(glm::vec3(100.0f, 50.0f, 0.0f));
    normalShader = new Shader("res/shaders/NormalShader.vs", "res/shaders/BlinPhongShader.fs");
    instanceShader = new Shader("res/shaders/InstanceShader.vs", "res/shaders/BlinPhongShader.fs");
    lightShader = new Shader("res/shaders/VertexShader.vs", "res/shaders/FragmentShader.fs");
    SkyboxShader = new Shader("res/shaders/SkyboxShader.vs", "res/shaders/SkyboxShader.fs");
    reflectionShader = new Shader("res/shaders/NormalShader.vs", "res/shaders/ReflectionShader.fs");
    refractionShader = new Shader("res/shaders/NormalShader.vs", "res/shaders/RefractionShader.fs");
    //normalShader = new Shader("res/shaders/NormalShader.vs", "res/shaders/FragmentShader.fs");
    //instanceShader = new Shader("res/shaders/InstanceShader.vs", "res/shaders/FragmentShader.fs");

    projection = glm::perspective(glm::radians(45.0f), (float)WINDOW_WIDTH / (float)WINDOW_HEIGHT, 0.1f, viewRange);

    normalShader->use();
    normalShader->setMat4("projection", projection);
    instanceShader->use();
    instanceShader->setMat4("projection", projection);
    lightShader->use();
    lightShader->setMat4("projection", projection);
    SkyboxShader->use();
    SkyboxShader->setInt("skybox", 0);
    SkyboxShader->setMat4("projection", projection);
    reflectionShader->use();
    reflectionShader->setInt("skybox", 0);
    reflectionShader->setMat4("projection", projection);
    refractionShader->use();
    refractionShader->setInt("skybox", 0);
    refractionShader->setFloat("coefR", 1.52);
    refractionShader->setFloat("coefG", 1.4);
    refractionShader->setFloat("coefB", 1.3);
    refractionShader->setMat4("projection", projection);

    //model = glm::mat4(1.0f);

    terrain = new Model("res/models/terrain.obj");
    cone1 = new Model("res/models/spot_light_cone.obj");
    cone2 = new Model("res/models/spot_light_cone.obj");
    sphere = new Model("res/models/point_light_sphere.obj");
    arrow = new Model("res/models/directional_light_arrow.obj");
    plane = new Model("res/models/plane.obj");


    instancesData[0] = new InstancesData;
    instancesData[1] = new InstancesData;

    float offset = 400.0f;

    for (int i = 0; i < numberOfRows; i++) {
        for (int j = 0; j < numberOfColumns; j++) {
            instancesData[0]->Positions.push_back(glm::vec3(i * offset, 0.0f, j * offset));
            instancesData[0]->Rotations.push_back(glm::vec3(0.0f, 0.0f, 0.0f));
            instancesData[0]->Scales.push_back(glm::vec3(1.0f, 1.0f, 1.0f));
            instancesData[0]->Usages.push_back(true);
        }
    }

    instancesData[0]->Positions[0] = glm::vec3(0.0f, 100.0f, 0.0f);

    instancesData[0]->Amount = instancesData[0]->Positions.size();
    instancesData[0]->UsedModelMatrices = new glm::mat4[instancesData[0]->Amount];

    for (int i = 0; i < instancesData[0]->Amount; i++) {
        instancesData[1]->Positions.push_back(glm::vec3(0.0f, 0.0f, 0.0f));
        instancesData[1]->Rotations.push_back(glm::vec3(0.0f, 0.0f, 0.0f));
        instancesData[1]->Scales.push_back(glm::vec3(1.0f, 1.0f, 1.0f));
        instancesData[1]->Usages.push_back(true);
    }
    instancesData[1]->Amount = instancesData[0]->Amount;
    instancesData[1]->UsedModelMatrices = new glm::mat4[instancesData[1]->Amount];


    instancesData[0]->Scales[1] = glm::vec3(1.5f, 1.5f, 1.5f);

    updateInstancesData();

    house = new Model("res/models/house2.obj", instancesData);

    glEnable(GL_DEPTH_TEST);
    //glPolygonMode(GL_FRONT_AND_BACK, GL_FILL);

    setUpScene();

    // Main loop
    while (!glfwWindowShouldClose(window))
    {
        float currentFrame = static_cast<float>(glfwGetTime());
        deltaTime = currentFrame - lastFrame;
        lastFrame = currentFrame;

        // Process I/O operations here
        input();

        // Update game objects' state here
        update();

        // OpenGL rendering code here
        //render();

        // Draw ImGui
        imgui_begin();
        imgui_render(); // edit this function to add your own ImGui controls
        imgui_end(); // this call effectively renders ImGui

        // End frame and swap buffers (double buffering)
        end_frame();
    }

    // Cleanup
    //delete camera;

    ImGui_ImplOpenGL3_Shutdown();
    ImGui_ImplGlfw_Shutdown();
    ImGui::DestroyContext();

    glfwDestroyWindow(window);
    glfwTerminate();

    return 0;
}

bool init()
{
    // Setup window
    glfwSetErrorCallback(glfw_error_callback);
    if (!glfwInit()) 
    {
        spdlog::error("Failed to initalize GLFW!");
        return false;
    }

    // GL 4.6 + GLSL 460
    glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, GL_VERSION_MAJOR);
    glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, GL_VERSION_MINOR);
    glfwWindowHint(GLFW_OPENGL_PROFILE,        GLFW_OPENGL_CORE_PROFILE);  // 3.2+ only
    glfwWindowHint(GLFW_OPENGL_FORWARD_COMPAT, GL_TRUE);            // 3.0+ only

    // Create window with graphics context
    window = glfwCreateWindow(WINDOW_WIDTH, WINDOW_HEIGHT, "Dear ImGui GLFW+OpenGL4 example", NULL, NULL);
    if (window == NULL)
    {
        spdlog::error("Failed to create GLFW Window!");
        return false;
    }

    glfwSetInputMode(window, GLFW_CURSOR, GLFW_CURSOR_DISABLED);

    glfwMakeContextCurrent(window);
    glfwSwapInterval(1); // Enable VSync - fixes FPS at the refresh rate of your screen

    bool err = !gladLoadGLLoader((GLADloadproc)glfwGetProcAddress);

    glfwSetCursorPosCallback(window, mouse_callback);

    glPolygonMode(GL_FRONT, GL_FILL);

    //glEnable(GL_BLEND);
    //glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
    //glBlendFunc(GL_SRC_ALPHA, GL_SRC_ALPHA);

    if (err)
    {
        spdlog::error("Failed to initialize OpenGL loader!");
        return false;
    }

    loadSkybox();

    return true;
}


void loadSkybox() {
    glGenTextures(1, &skyboxTextureID);
    glBindTexture(GL_TEXTURE_CUBE_MAP, skyboxTextureID);

    int width, height, nrChannels;
    for (unsigned int i = 0; i < faces.size(); i++)
    {
        unsigned char* data = stbi_load(faces[i].c_str(), &width, &height, &nrChannels, 0);
        if (data)
        {
            glTexImage2D(GL_TEXTURE_CUBE_MAP_POSITIVE_X + i,
                0, GL_RGB, width, height, 0, GL_RGB, GL_UNSIGNED_BYTE, data
            );
            stbi_image_free(data);
        }
        else
        {
            std::cout << "Cubemap texture failed to load at path: " << faces[i] << std::endl;
            stbi_image_free(data);
        }
    }

    glTexParameteri(GL_TEXTURE_CUBE_MAP, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
    glTexParameteri(GL_TEXTURE_CUBE_MAP, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
    glTexParameteri(GL_TEXTURE_CUBE_MAP, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
    glTexParameteri(GL_TEXTURE_CUBE_MAP, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);
    glTexParameteri(GL_TEXTURE_CUBE_MAP, GL_TEXTURE_WRAP_R, GL_CLAMP_TO_EDGE);

    float skyboxVertices[] = {
        // positions          
        -1.0f,  1.0f, -1.0f,
        -1.0f, -1.0f, -1.0f,
         1.0f, -1.0f, -1.0f,
         1.0f, -1.0f, -1.0f,
         1.0f,  1.0f, -1.0f,
        -1.0f,  1.0f, -1.0f,

        -1.0f, -1.0f,  1.0f,
        -1.0f, -1.0f, -1.0f,
        -1.0f,  1.0f, -1.0f,
        -1.0f,  1.0f, -1.0f,
        -1.0f,  1.0f,  1.0f,
        -1.0f, -1.0f,  1.0f,

         1.0f, -1.0f, -1.0f,
         1.0f, -1.0f,  1.0f,
         1.0f,  1.0f,  1.0f,
         1.0f,  1.0f,  1.0f,
         1.0f,  1.0f, -1.0f,
         1.0f, -1.0f, -1.0f,

        -1.0f, -1.0f,  1.0f,
        -1.0f,  1.0f,  1.0f,
         1.0f,  1.0f,  1.0f,
         1.0f,  1.0f,  1.0f,
         1.0f, -1.0f,  1.0f,
        -1.0f, -1.0f,  1.0f,

        -1.0f,  1.0f, -1.0f,
         1.0f,  1.0f, -1.0f,
         1.0f,  1.0f,  1.0f,
         1.0f,  1.0f,  1.0f,
        -1.0f,  1.0f,  1.0f,
        -1.0f,  1.0f, -1.0f,

        -1.0f, -1.0f, -1.0f,
        -1.0f, -1.0f,  1.0f,
         1.0f, -1.0f, -1.0f,
         1.0f, -1.0f, -1.0f,
        -1.0f, -1.0f,  1.0f,
         1.0f, -1.0f,  1.0f
    };
    glGenVertexArrays(1, &skyboxVAO);
    glGenBuffers(1, &skyboxVBO);
    glBindVertexArray(skyboxVAO);
    glBindBuffer(GL_ARRAY_BUFFER, skyboxVBO);
    glBufferData(GL_ARRAY_BUFFER, sizeof(skyboxVertices), &skyboxVertices, GL_STATIC_DRAW);
    glEnableVertexAttribArray(0);
    glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 3 * sizeof(float), (void*)0);

    glBindVertexArray(0);

}


void drawSkybox() {
    glDepthFunc(GL_LEQUAL);

    SkyboxShader->use();

    glBindVertexArray(skyboxVAO);
    glActiveTexture(GL_TEXTURE0);
    glBindTexture(GL_TEXTURE_CUBE_MAP, skyboxTextureID);
    glDrawArrays(GL_TRIANGLES, 0, 36);

    glBindVertexArray(0);
    glDepthFunc(GL_LESS);
}


void setUpScene() {
    PointLightData* pointLightData = new PointLightData;
    pointLightData->PointLightOn = true;
    pointLightData->PointLightColor = glm::vec4(1, 0, 0, 1);
    pointLightData->PointLightPosition = glm::vec3(0, 800, 0);
    pointLightData->PointLightPower = 2.0f;
    pointLightData->PointLightRadius = 2000.0f;

    SpotLightData* spotLightData1 = new SpotLightData;
    spotLightData1->SpotLightOn = true;
    spotLightData1->SpotLightColor = glm::vec4(1, 1, 0, 1);
    spotLightData1->SpotLightPos = glm::vec3(800, 200, 400);
    spotLightData1->SpotLightDir = glm::vec3(-1, 0, 0);
    spotLightData1->coneAngle = 0.785f;
    spotLightData1->concentration = 1.0f;
    spotLightData1->SpotLightRadius = 1000.0f;
    spotLightData1->SpotLightPower = 2.0f;
             
    SpotLightData* spotLightData2 = new SpotLightData;
    spotLightData2->SpotLightOn = true;
    spotLightData2->SpotLightColor = glm::vec4(0, 1, 1, 1);
    spotLightData2->SpotLightPos = glm::vec3(-800, 200, 400);
    spotLightData2->SpotLightDir = glm::vec3(1, 0, 0);
    spotLightData2->coneAngle = 0.785f;
    spotLightData2->concentration = 1.0f;
    spotLightData2->SpotLightRadius = 1000.0f;
    spotLightData2->SpotLightPower = 2.0f;

    DirectionalLightData* directionalLightData = new DirectionalLightData;
    directionalLightData->DirectionalLightOn = true;
    directionalLightData->DirectionalLightColor = glm::vec4(0, 1, 0, 1);
    glm::vec3 dir = glm::vec3(-0.707, -0.707, 0);
    directionalLightData->DirectionalLightDir = dir;
    directionalLightData->DirectionalLightPower = 1.0f;

    Transform* transform = new Transform();

    rootGameObject = new GameObject(transform);


    transform = new Transform();
    transform->setScale(glm::vec3(10, 10, 10));
    pointLight = new PointLight(sphere, lightShader, transform, pointLightData, 0, 0, 500.0f, normalShader, instanceShader);

    transform = new Transform();
    transform->setScale(glm::vec3(10, 10, 10));
    spotLight1 = new SpotLight(cone1, lightShader, transform, spotLightData1, "1");

    transform = new Transform();
    transform->setScale(glm::vec3(10, 10, 10));
    spotLight2 = new SpotLight(cone2, lightShader, transform, spotLightData2, "2");

    transform = new Transform();
    transform->setTranslate(glm::vec3(0, 800, 0));
    transform->setScale(glm::vec3(10, 10, 10));
    directionalLight = new DirectionalLight(arrow, lightShader, transform, directionalLightData);

    updatePointLightData();
    updateSpotLight1Data();
    updateSpotLight2Data();
    updateDirectionalLightData();
    updateAditionalLightData();

    rootGameObject->addChild(pointLight);
    rootGameObject->addChild(spotLight1);
    rootGameObject->addChild(spotLight2);
    rootGameObject->addChild(directionalLight);

    //transform->setScale(glm::vec3(100, 100, 100));
    transform = new Transform();
    GameObject* Terrain = new GameObject(terrain, normalShader, transform);
    rootGameObject->addChild(Terrain);

    //transform->setScale(glm::vec3(0.01, 0.01, 0.01));
    transform = new Transform();
    GameObject* House = new GameObject(house, instanceShader, transform);//instanceShader
    Terrain->addChild(House);


    transform = new Transform();
    transform->setTranslate(glm::vec3(-200, 200, -200));
    //transform->setScale(glm::vec3(0.1, 0.1, 0.1));
    Plane = new GameObject(plane, normalShader, transform);//instanceShader refractionShader reflectionShader
    rootGameObject->addChild(Plane);

    transform = new Transform();
    transform->setTranslate(glm::vec3(-50, 0, -150));
    GameObject* PlaneC = new GameObject(plane, reflectionShader, transform);
    Plane->addChild(PlaneC);

    transform = new Transform();
    transform->setTranslate(glm::vec3(-50, 0, 150));
    PlaneC = new GameObject(plane, refractionShader, transform);
    Plane->addChild(PlaneC);

    Player = new PlanePLayerController(Plane);
    cameraObj = new CameraObject(glm::vec3(-300, 100, 0));
    Plane->addChild(cameraObj);
}

void init_imgui()
{
    // Setup Dear ImGui binding
    IMGUI_CHECKVERSION();
    ImGui::CreateContext();
    ImGuiIO& io = ImGui::GetIO(); (void)io;

    ImGui_ImplGlfw_InitForOpenGL(window, true);
    ImGui_ImplOpenGL3_Init(glsl_version);

    // Setup style
    ImGui::StyleColorsDark();
    //ImGui::StyleColorsClassic();
}

void input()
{
    if (glfwGetKey(window, GLFW_KEY_ESCAPE) == GLFW_PRESS) {
        glfwSetWindowShouldClose(window, true);
    }/**/

    if (glfwGetKey(window, GLFW_KEY_E) == GLFW_PRESS) {
        moveView = true;
        glfwSetInputMode(window, GLFW_CURSOR, GLFW_CURSOR_DISABLED);
    }

    if (glfwGetKey(window, GLFW_KEY_Q) == GLFW_PRESS) {
        moveView = false;
        glfwSetInputMode(window, GLFW_CURSOR, GLFW_CURSOR_NORMAL);
    }

    /*if (moveView) {
        if (glfwGetKey(window, GLFW_KEY_W) == GLFW_PRESS) {
            camera->ProcessKeyboard(FORWARD, deltaTime);
            updateView();
        }
        if (glfwGetKey(window, GLFW_KEY_S) == GLFW_PRESS) {
            camera->ProcessKeyboard(BACKWARD, deltaTime);
            updateView();
        }
        if (glfwGetKey(window, GLFW_KEY_A) == GLFW_PRESS) {
            camera->ProcessKeyboard(LEFT, deltaTime);
            updateView();
        }
        if (glfwGetKey(window, GLFW_KEY_D) == GLFW_PRESS) {
            camera->ProcessKeyboard(RIGHT, deltaTime);
            updateView();
        }
    }*/

    bool leftWUp = false;
    bool leftWDown = false;
    bool rightWUp = false;
    bool rightWDown = false;

    if (glfwGetKey(window, GLFW_KEY_W) == GLFW_PRESS) {
        leftWUp = true;
    }
    if (glfwGetKey(window, GLFW_KEY_S) == GLFW_PRESS) {
        leftWDown = true;
    }
    if (glfwGetKey(window, GLFW_KEY_E) == GLFW_PRESS) {
        rightWUp = true;
    }
    if (glfwGetKey(window, GLFW_KEY_D) == GLFW_PRESS) {
        rightWDown = true;
    }
    if (glfwGetKey(window, GLFW_KEY_A) == GLFW_PRESS) {
        Player->ProcessKeyboard(PlanePLayerController::PlaneMovement::ROT_LEFT, deltaTime);
    }
    if (glfwGetKey(window, GLFW_KEY_F) == GLFW_PRESS) {
        Player->ProcessKeyboard(PlanePLayerController::PlaneMovement::ROT_RIGHT, deltaTime);
    }
    if (glfwGetKey(window, GLFW_KEY_SPACE) == GLFW_PRESS) {
        Player->ProcessKeyboard(PlanePLayerController::PlaneMovement::PROPEL, deltaTime);
    }

    if (leftWUp && rightWUp) {
        Player->ProcessKeyboard(PlanePLayerController::PlaneMovement::ROT_UP, deltaTime);
    }
    if (leftWDown && rightWDown) {
        Player->ProcessKeyboard(PlanePLayerController::PlaneMovement::ROT_DOWN, deltaTime);
    }

    if (leftWUp && (!rightWUp)) {
        Player->ProcessKeyboard(PlanePLayerController::PlaneMovement::ROLL_RIGHT, deltaTime);
    }
    if (rightWUp && (!leftWUp)) {
        Player->ProcessKeyboard(PlanePLayerController::PlaneMovement::ROLL_LEFT, deltaTime);
    }
    if (leftWDown && (!rightWDown)) {
        Player->ProcessKeyboard(PlanePLayerController::PlaneMovement::ROLL_LEFT, deltaTime);
    }
    if (rightWDown && (!leftWDown)) {
        Player->ProcessKeyboard(PlanePLayerController::PlaneMovement::ROLL_RIGHT, deltaTime);
    }

    Player->UpdateVectors();

    //updateView();
}

void update()
{
    rootGameObject->Update();

    pointLight->Update();

    Player->Update(deltaTime);
    cameraObj->Up = Player->getUp();

    updateView();
}

void imgui_begin()
{
    // Start the Dear ImGui frame
    ImGui_ImplOpenGL3_NewFrame();
    ImGui_ImplGlfw_NewFrame();
    ImGui::NewFrame();
}

void imgui_render()
{
    ImGui::Begin("Oœwietlenie");

    ImGui::Text("Opcje oœwietlenia");
    ImGui::InputFloat("Wspó³czynnik jasnoœci specular", &n);

    PointLightData* pLight = pointLight->getLightData();
    SpotLightData* s1Light = spotLight1->getLightData();
    SpotLightData* s2Light = spotLight2->getLightData();
    DirectionalLightData* dLight = directionalLight->getLightData();

    ImGui::Separator();
    ImGui::BeginChild("Point Light", ImVec2(0, 150));
    ImGui::Checkbox("PointLightOn", &pLight->PointLightOn);
    ImGui::ColorEdit4("PointLightColor", (float*)&pLight->PointLightColor);
    ImGui::InputFloat3("PointLightPosition", (float*)&pLight->PointLightPosition);
    ImGui::InputFloat("PointLightPower", &pLight->PointLightPower);
    ImGui::InputFloat("PointLightRadius", &pLight->PointLightRadius);
    ImGui::EndChild();

    ImGui::Separator();
    ImGui::BeginChild("Spot Light 1", ImVec2(0, 200));
    ImGui::Checkbox("SpotLightOn", &s1Light->SpotLightOn);
    ImGui::ColorEdit4("SpotLightColor", (float*)&s1Light->SpotLightColor);
    ImGui::InputFloat3("SpotLightPos", (float*)&s1Light->SpotLightPos);
    ImGui::InputFloat3("SpotLightDir", (float*)&s1Light->SpotLightDir);
    ImGui::InputFloat("coneAngle", &s1Light->coneAngle);
    ImGui::InputFloat("concentration", &s1Light->concentration);
    ImGui::InputFloat("SpotLightPower", &s1Light->SpotLightPower);
    ImGui::InputFloat("SpotLightRadius", &s1Light->SpotLightRadius);
    ImGui::EndChild();

    ImGui::Separator();
    ImGui::BeginChild("Spot Light 2", ImVec2(0, 200));
    ImGui::Checkbox("SpotLightOn", &s2Light->SpotLightOn);
    ImGui::ColorEdit4("SpotLightColor", (float*)&s2Light->SpotLightColor);
    ImGui::InputFloat3("SpotLightPos", (float*)&s2Light->SpotLightPos);
    ImGui::InputFloat3("SpotLightDir", (float*)&s2Light->SpotLightDir);
    ImGui::InputFloat("coneAngle", &s2Light->coneAngle);
    ImGui::InputFloat("concentration", &s2Light->concentration);
    ImGui::InputFloat("SpotLightPower", &s2Light->SpotLightPower);
    ImGui::InputFloat("SpotLightRadius", &s2Light->SpotLightRadius);
    ImGui::EndChild();

    ImGui::Separator();
    ImGui::BeginChild("Directional Light", ImVec2(0, 100));
    ImGui::Checkbox("DirectionalLightOn", &dLight->DirectionalLightOn);
    ImGui::ColorEdit4("DirectionalLightColor", (float*)&dLight->DirectionalLightColor);
    ImGui::InputFloat3("DirectionalLightDir", (float*)&dLight->DirectionalLightDir);
    ImGui::InputFloat("DirectionalLightPower", &dLight->DirectionalLightPower);
    ImGui::EndChild();

    ImGui::Separator();
    if (ImGui::Button("Aktualizuj")) {
        updatePointLightData();
        updateSpotLight1Data();
        updateSpotLight2Data();
        updateDirectionalLightData();
        updateAditionalLightData();
    }

    ImGui::End();


    static int px = -1;
    static int x = 0;
    static int py = -1;
    static int y = 0;

    static bool visibility;
    static glm::vec3 position;
    static glm::vec3 rotation;
    static glm::vec3 scale;

    static bool roofvisibility;
    static glm::vec3 roofposition;
    static glm::vec3 roofrotation;
    static glm::vec3 roofscale;

    ImGui::Begin("Instancjonowanie");

    ImGui::Text("Transformowanie instancji");

    ImGui::Separator();
    ImGui::Text("Wspolrzedne instacji:");
    ImGui::SliderInt("Numer kolumny:", &x, 0, numberOfColumns);
    ImGui::SliderInt("Numer wiersza:", &y, 0, numberOfRows);


    if (px != x || py != y) {
        visibility = instancesData[0]->Usages[y * numberOfColumns + x];
        position = instancesData[0]->Positions[y * numberOfColumns + x];
        rotation = instancesData[0]->Rotations[y * numberOfColumns + x];
        scale = instancesData[0]->Scales[y * numberOfColumns + x];

        roofvisibility = instancesData[1]->Usages[y * numberOfColumns + x];
        roofposition = instancesData[1]->Positions[y * numberOfColumns + x];
        roofrotation = instancesData[1]->Rotations[y * numberOfColumns + x];
        roofscale = instancesData[1]->Scales[y * numberOfColumns + x];

        px = x;
        py = y;
    }


    ImGui::Separator();
    ImGui::Separator();
    ImGui::Text("DOM:");

    ImGui::Separator();
    ImGui::Checkbox("Widocznosc domu", &visibility);
    instancesData[0]->Usages[y * numberOfColumns + x] = visibility;

    ImGui::InputFloat3("Pozycja", (float*)&position);
    ImGui::InputFloat3("Obrot", (float*)&rotation);
    ImGui::InputFloat3("Skala", (float*)&scale);
    instancesData[0]->Positions[y * numberOfColumns + x] = position;
    instancesData[0]->Rotations[y * numberOfColumns + x] = rotation;
    instancesData[0]->Scales[y * numberOfColumns + x] = scale;

    ImGui::Separator();
    ImGui::Text("DACH:");

    ImGui::Separator();
    ImGui::Checkbox("Widocznosc dachu", &roofvisibility);
    instancesData[1]->Usages[y * numberOfColumns + x] = roofvisibility;

    ImGui::InputFloat3("Pozycja dachu", (float*)&roofposition);
    ImGui::InputFloat3("Obrot dachu", (float*)&roofrotation);
    ImGui::InputFloat3("Skala dachu", (float*)&roofscale);
    instancesData[1]->Positions[y * numberOfColumns + x] = roofposition;
    instancesData[1]->Rotations[y * numberOfColumns + x] = roofrotation;
    instancesData[1]->Scales[y * numberOfColumns + x] = roofscale;


    ImGui::Separator();
    if (ImGui::Button("Aktualizuj instancje")) {
        updateInstancesData();

        InstanceMesh* im;
        for (auto m : house->meshes) {
            im = (InstanceMesh*)m;
            im->setUpInstancesData();
        }
    }

    ImGui::End();
}

void imgui_end()
{
    ImGui::Render();
    int display_w, display_h;
    glfwMakeContextCurrent(window);
    glfwGetFramebufferSize(window, &display_w, &display_h);

    glViewport(0, 0, display_w, display_h);
    glClearColor(clear_color.x, clear_color.y, clear_color.z, clear_color.w);
    glClear(GL_COLOR_BUFFER_BIT);
    glClear(GL_DEPTH_BUFFER_BIT);

    render();

    ImGui_ImplOpenGL3_RenderDrawData(ImGui::GetDrawData());
}

void render()
{
    rootGameObject->Draw(model);
    drawSkybox();
}

void end_frame()
{
    // Poll and handle events (inputs, window resize, etc.)
    // You can read the io.WantCaptureMouse, io.WantCaptureKeyboard flags to tell if dear imgui wants to use your inputs.
    // - When io.WantCaptureMouse is true, do not dispatch mouse input data to your main application.
    // - When io.WantCaptureKeyboard is true, do not dispatch keyboard input data to your main application.
    // Generally you may always pass all inputs to dear imgui, and hide them from your application based on those two flags.
    glfwPollEvents();
    glfwMakeContextCurrent(window);
    glfwSwapBuffers(window);
}

void updateView() {
    glm::vec3 camPos = glm::vec3(cameraObj->getAbsoluteTransformationMatrix() * glm::vec4(0, 0, 0, 1));

    //view = camera->GetViewMatrix();
    view = cameraObj->GetViewMatrix();
    normalShader->use();
    normalShader->setMat4("view", view);
    //normalShader->setVec3("PlayerPosition", camera->Position);
    normalShader->setVec3("PlayerPosition", camPos);
    instanceShader->use();
    instanceShader->setMat4("view", view);
    //instanceShader->setVec3("PlayerPosition", camera->Position); 
    instanceShader->setVec3("PlayerPosition", camPos);
    lightShader->use();
    lightShader->setMat4("view", view);
    SkyboxShader->use();
    glm::mat4 mview = glm::mat4(glm::mat3(view));
    SkyboxShader->setMat4("view", mview);
    //->setVec3("PlayerPosition", camera->Position);
    reflectionShader->use();
    reflectionShader->setMat4("view", view);
    reflectionShader->setVec3("cameraPos", camPos);
    //reflectionShader->setVec3("cameraPos", camera->Position);
    refractionShader->use();
    refractionShader->setMat4("view", view);
    refractionShader->setVec3("cameraPos", camPos);
    //refractionShader->setVec3("cameraPos", camera->Position);
}

void updateInstancesData() {
    glm::mat4 matrix = glm::mat4(1.0f);

    instancesData[0]->UsedAmount = 0;
    instancesData[1]->UsedAmount = 0;

    for (int i = 0; i < instancesData[0]->Amount; i++) {
        matrix = glm::mat4(1.0f);
        if (instancesData[0]->Usages[i]) {
            matrix = glm::translate(matrix, instancesData[0]->Positions[i]);
            matrix = glm::rotate(matrix, instancesData[0]->Rotations[i].x, glm::vec3(1.0f, 0.0f, 0.0f));
            matrix = glm::rotate(matrix, instancesData[0]->Rotations[i].y, glm::vec3(0.0f, 1.0f, 0.0f));
            matrix = glm::rotate(matrix, instancesData[0]->Rotations[i].z, glm::vec3(0.0f, 0.0f, 1.0f));
            matrix = glm::scale(matrix, instancesData[0]->Scales[i]);

            instancesData[0]->UsedModelMatrices[instancesData[0]->UsedAmount] = matrix;
            instancesData[0]->UsedAmount += 1;
        }

        matrix = glm::mat4(1.0f);
        if (instancesData[0]->Usages[i] && instancesData[1]->Usages[i]) {
            matrix = glm::translate(matrix, instancesData[1]->Positions[i]);
            matrix = glm::rotate(matrix, instancesData[1]->Rotations[i].x, glm::vec3(1.0f, 0.0f, 0.0f));
            matrix = glm::rotate(matrix, instancesData[1]->Rotations[i].y, glm::vec3(0.0f, 1.0f, 0.0f));
            matrix = glm::rotate(matrix, instancesData[1]->Rotations[i].z, glm::vec3(0.0f, 0.0f, 1.0f));
            matrix = glm::scale(matrix, instancesData[1]->Scales[i]);

            instancesData[1]->UsedModelMatrices[instancesData[1]->UsedAmount] = instancesData[0]->UsedModelMatrices[instancesData[0]->UsedAmount - 1] * matrix;
            instancesData[1]->UsedAmount += 1;
        }
    }
}

void updatePointLightData() {
    pointLight->UpdateLightData();
}

void updateSpotLight1Data() {
    spotLight1->UpdateLightData(normalShader, instanceShader);
}

void updateSpotLight2Data() {
    spotLight2->UpdateLightData(normalShader, instanceShader);
}

void updateDirectionalLightData() {
    directionalLight->UpdateLightData(normalShader, instanceShader);
}

void updateAditionalLightData() {
    normalShader->use();

    //normalShader->setFloat("Kd", 1.0f);
    //normalShader->setFloat("Ks", 0.5f);
    normalShader->setVec4("AmbientColor", glm::vec4(0, 0, 0, 1));
    normalShader->setFloat("Ka", 0.0f);
    normalShader->setFloat("n", 3);

    instanceShader->use();

    //instanceShader->setFloat("Kd", 1.0f);
    //instanceShader->setFloat("Ks", 0.5f);
    instanceShader->setVec4("AmbientColor", glm::vec4(0, 0, 0, 1));
    instanceShader->setFloat("Ka", 0.0f);
    instanceShader->setFloat("n", 3);/**/
}

void mouse_callback(GLFWwindow* window, double xposIn, double yposIn)
{
    /*if (moveView) {
        float xpos = static_cast<float>(xposIn);
        float ypos = static_cast<float>(yposIn);

        if (firstMouse)
        {
            lastX = xpos;
            lastY = ypos;
            firstMouse = false;
        }

        float xoffset = xpos - lastX;
        float yoffset = lastY - ypos;
        lastX = xpos;
        lastY = ypos;

        camera->ProcessMouseMovement(xoffset, yoffset);
        updateView();
    }*/
}
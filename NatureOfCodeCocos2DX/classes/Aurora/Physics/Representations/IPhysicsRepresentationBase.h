#ifndef NatureOfCodeCocos2DX_IPhysicsRepresentationBase_h
#define NatureOfCodeCocos2DX_IPhysicsRepresentationBase_h



#include "../Implementations/Force.h"



namespace Aurora {
	namespace Physics {


		class IPhysicsRepresentationBase : public IPhysicsBase
		{
		protected:
			std::shared_ptr<Physics::Force> objectPhysics;
			virtual void init() override
			{
				this->objectPhysics = std::make_shared<Physics::Force>();
			}
		public:
			IPhysicsRepresentationBase();
			virtual ~IPhysicsRepresentationBase();

			std::shared_ptr<Physics::Force> ObjectPhysics() const { return objectPhysics; }
			template<typename T>
			void ObjectPhysics(T &&value) { objectPhysics = std::forward<T>(value); }

			

		};

		

	}; // END OF NAMESPACE Random
}; // END OF NAMESPACE Aurora


#endif